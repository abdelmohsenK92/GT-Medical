using GT_Medical.Abstractions;
using GT_Medical.Helper;
using GT_Medical.Services;
using LibVLCSharp.Shared;
using LibVLCSharp.WinForms;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using Timer = System.Windows.Forms.Timer;

namespace GT_Medical.UI
{
    public partial class VlcPlayerControl : UserControl, IVideoSurfaceUi, ITransientService
    {
        // === لا يوجد LibVLC ولا MediaPlayer هنا ===

        // Timers (UI only)
        private Timer _uiTimer;          // soft refresh for labels/seek animation
        private Timer _overlayTimer;     // auto-hide controls
        private int _autoHideMs = 1800;
        private DateTime _lastMouseMove = DateTime.UtcNow;
        private CrossThreadInvoker _invoker ;
        // Cached icons
        private Image _icoPlay, _icoPause, _icoReplay, _icoFullscreen, _icoExitFullscreen , _icoVolume;

        // State cache from service
        private bool _isPlaying;
        private bool _isFullscreen = false;
        private Form _fsForm;
        private Control _oldParent;
        private DockStyle _oldDock;
        private Rectangle _oldBounds;
        private long _lengthMs = -1;
        private long _currentMs = -1;
        private float _position01 = 0f;
        private int _volume = 70;
        private bool _isDraggingSeek = false;

        public VlcPlayerControl()
        {
            InitializeComponent();
            _invoker = new CrossThreadInvoker(this);
            InitializeUiOnly();
        }
        
        #region IVideoSurfaceUi
        [Browsable(false)]
        public VideoView VideoSurface => videoView;

        public event Action TogglePlayPauseRequested;
        public event Action ReplayRequested;
        public event Action FullscreenToggleRequested;
        public event Action<int> VolumeChanged;
        public event Action<float> SeekRequested;
        public event EventHandler VideoSurfaceInitialized;
        public void NotifyFinishInitialization() => VideoSurfaceInitialized?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Service pushes current state here; the control reflects it.
        /// </summary>
        public void SetPlaybackState(bool isPlaying, long currentMs, long lengthMs, int volume, float position01)
        {
            _isPlaying = isPlaying;
            _currentMs = currentMs;
            _lengthMs = lengthMs;
            _volume = Math.Max(0, Math.Min(100, volume));
            _position01 = Math.Max(0f, Math.Min(1f, position01));
            _currentMs = currentMs;
            Debug.WriteLine(currentMs + " : " + lengthMs);
            // Reflect quickly in UI
            UpdateUi(refreshSeek: true, refreshTime: true, refreshButtons: true, refreshVolume: true);
        }
       
        public void ShowOverlayTemporarily()
        {
            _invoker.RunOnUi(() =>
            {
                pnlControls.Visible = true;
                btnCenter.Visible = true;
                _lastMouseMove = DateTime.UtcNow;
            });
        }
        #endregion

        #region Init (UI-only)
        private void InitializeUiOnly()
        {
            _invoker.RunOnUi(() =>
            {
                // Load icons from Assets (once)
                TryLoadIcons();

                // timers
                _uiTimer = new Timer { Interval = 200 };
                _uiTimer.Tick += (s, e) => UpdateUi();
                _uiTimer.Start();

                _overlayTimer = new Timer { Interval = 300 };
                _overlayTimer.Tick += (s, e) => AutoHideUi();
                _overlayTimer.Start();

                // mouse for overlay
                this.MouseMove += Any_MouseMove;
                videoView.MouseHover += (s, e) => ShowOverlayTemporarily();
                videoView.MouseMove += Any_MouseMove;
                pnlControls.MouseMove += Any_MouseMove;
                btnCenter.MouseMove += Any_MouseMove;
                // clicks -> raise events (service handles them)
                videoView.MouseClick += (s, e) => { if (e.Button == MouseButtons.Left) TogglePlayPauseRequested?.Invoke(); };
                btnPlayPause.Click += (s, e) => TogglePlayPauseRequested?.Invoke();
                btnReplay.Click += (s, e) => { ReplayRequested?.Invoke(); ShowOverlayTemporarily(); };
                // داخل InitializeUiOnly() أو حيث تربط باقي الأحداث
                btnFullscreen.Click += (s, e) => ToggleFullscreen();

                // لو عندك Event FullscreenToggleRequested على الكنترول نفسه:
                this.FullscreenToggleRequested += () => ToggleFullscreen();
                btnCenter.Click += (s, e) => TogglePlayPauseRequested?.Invoke();

                // seek -> raise event with percentage
                tbSeek.Scroll += (s, e) =>
                {
                    _isDraggingSeek = true;
                    var p = tbSeek.Value / 1000f;
                    SeekRequested?.Invoke(p);
                    _isDraggingSeek = false;
                };

                // volume -> raise event 0..100
                tbVolume.Scroll += (s, e) =>
                {
                    _volume = tbVolume.Value;
                    VolumeChanged?.Invoke(_volume);
                    _lastMouseMove = DateTime.UtcNow;
                };
                tbVolume.Value = _volume;

                // center overlay
                root.Resize += (s, e) => CenterOverlayButton();
                CenterOverlayButton();
                VideoSurfaceInitialized += (s,e) => 
                {
                    videoView.MediaPlayer.EndReached += (s, e) => ShowOverlayTemporarily();
                };
                // keyboard (optional UI shortcut)
                this.TabStop = true;
            });
            this.KeyDown += (s, e) =>
                {
                    if (e.KeyCode == Keys.Space)
                    {
                        TogglePlayPauseRequested?.Invoke();
                        e.Handled = true; e.SuppressKeyPress = true;
                    }
                    else if (e.KeyCode == Keys.F)
                    {
                        FullscreenToggleRequested?.Invoke();
                        e.Handled = true; e.SuppressKeyPress = true;
                    }
                    else if (e.KeyCode == Keys.Left)
                    {
                        // Ask service to seek -5s: we don't know absolute length here, service can translate
                        // Alternatively, you can keep it as is; many apps rely on UI -> fixed delta by service.
                        ShowOverlayTemporarily();
                    }
                    else if (e.KeyCode == Keys.Right)
                    {
                        ShowOverlayTemporarily();
                    }
                    else if (e.KeyCode == Keys.Up)
                    {
                        VolumeChanged?.Invoke(Math.Min(100, _volume + 5));
                        ShowOverlayTemporarily();
                    }
                    else if (e.KeyCode == Keys.Down)
                    {
                        VolumeChanged?.Invoke(Math.Max(0, _volume - 5));
                        ShowOverlayTemporarily();
                    }
                    else if (e.KeyCode == Keys.Escape)
                    {
                        if (_fsMode == FullscreenMode.InsideForm
                        && _isFullInsideForm)
                            ExitFullscreenInsideForm();
                        if (_fsMode == FullscreenMode.SeparateForm
                        && _isFullscreen)
                            ExitFullscreenSeparateForm();
                    }
                };
           
        }

        private void TryLoadIcons()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string assets = Path.Combine(baseDir, "Assets");

            _icoPlay = Properties.Resources.play_button;
            _icoPause = Properties.Resources.pause_button;
            _icoReplay = Properties.Resources.replay__1_;
            _icoFullscreen = Properties.Resources.expand;
            _icoExitFullscreen = Properties.Resources.minimize;
            _icoVolume = Properties.Resources.medium_volume;

            btnPlayPause.BackgroundImage = _icoPlay;
            btnReplay.BackgroundImage = _icoReplay;
            btnFullscreen.BackgroundImage = _icoFullscreen;
            picVolume.BackgroundImage = _icoVolume;
            btnCenter.BackgroundImage = _icoPlay;

            btnPlayPause.Text = _icoPlay == null ? "▶" : "";
            btnReplay.Text = _icoReplay == null ? "⟳" : "";
            btnFullscreen.Text = _icoFullscreen == null ? "⤢" : "";
            btnCenter.Text = _icoPlay == null ? "▶" : "";
        }

        #endregion

        #region UI logic
        private void UpdateUi(bool refreshSeek = false, bool refreshTime = false, bool refreshButtons = false, bool refreshVolume = false)
        {
            _invoker.RunOnUi(() =>
            {
                if (refreshButtons)
                {
                    // center and play/pause icons/text
                    btnPlayPause.BackgroundImage = _isPlaying ? _icoPause : _icoPlay;
                    if (btnPlayPause.BackgroundImage == null) btnPlayPause.Text = _isPlaying ? "❚❚" : "▶";

                    btnCenter.BackgroundImage = _isPlaying ? _icoPause : _icoPlay;
                    if (btnCenter.BackgroundImage == null) btnCenter.Text = _isPlaying ? "❚❚" : "▶";

                    // keep center visible if paused
                    if (!_isPlaying) ShowOverlayTemporarily();
                }

                if (refreshTime)
                {
                    if (_lengthMs > 0 && _currentMs >= 0)
                    {
                        var cur = TimeSpan.FromMilliseconds(_currentMs);
                        var tot = TimeSpan.FromMilliseconds(_lengthMs);
                        lblTime.Text = $"{FormatTime(cur)} / {FormatTime(tot)}";
                    }
                    else
                    {
                        lblTime.Text = "--:-- / --:--";
                    }
                }

                if (refreshSeek && !_isDraggingSeek)
                {
                    int pos = (int)Math.Max(0, Math.Min(1000, _position01 * 1000));
                    if (Math.Abs(tbSeek.Value - pos) > 2) tbSeek.Value = pos;
                }

                if (refreshVolume)
                {
                    if (tbVolume.Value != _volume) tbVolume.Value = _volume;
                }
            });
        }

        private void AutoHideUi()
        {
            _invoker.RunOnUi(() =>
            {
                if (_isPlaying)
                {
                    var idleMs = (DateTime.UtcNow - _lastMouseMove).TotalMilliseconds;
                    if (idleMs >= _autoHideMs)
                    {
                        pnlControls.Visible = false;
                        btnCenter.Visible = false;
                    }
                }
            });
        }

        private void Any_MouseMove(object sender, MouseEventArgs e)
        {
            _invoker.RunOnUi(() =>
            {
                _lastMouseMove = DateTime.UtcNow;
                ShowOverlayTemporarily();
            });
        }

        private void CenterOverlayButton()
        {
            _invoker.RunOnUi(() =>
            {
                btnCenter.Left = (root.Width - btnCenter.Width) / 2;
                btnCenter.Top = (root.Height - btnCenter.Height) / 2;
            });
        }
        /// <summary>
        /// Toggle fullscreen according to Mode (InsideForm or SeparateForm).
        /// </summary>
        public void ToggleFullscreen()
        {
            if (_fsMode == FullscreenMode.InsideForm)
            {
                if (!_isFullInsideForm) 
                    EnterFullscreenInsideForm();
                else ExitFullscreenInsideForm();
            }
            else // SeparateForm
            {
                if (!_isFullscreen) EnterFullscreenSeparateForm();
                else ExitFullscreenSeparateForm();
            }
            ShowOverlayTemporarily();
        }
        private FullscreenMode _fsMode = FullscreenMode.SeparateForm; // default; change as you like
        [Browsable(true)]
        [Category("Behavior")]
        [Description("How the control enters fullscreen: InsideForm or SeparateForm")]
        public FullscreenMode Mode
        {
            get => _fsMode;
            set => _fsMode = value;
        }
        // For InsideForm
        private bool _isFullInsideForm;
        private Control[] _hiddenSiblings;

        // Win32 (optional dragging of separate fullscreen)
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;
        [System.Runtime.InteropServices.DllImport("user32.dll")] private static extern bool ReleaseCapture();
        [System.Runtime.InteropServices.DllImport("user32.dll")] private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        /// <summary>
        /// Fullscreen inside current form: hide siblings and Dock=Fill.
        /// </summary>
        private void EnterFullscreenInsideForm()
        {
            var parent = this.Parent;
            if (parent == null) return;

            // Hide siblings except this
            _hiddenSiblings = parent.Controls.Cast<Control>().Where(c => c != this && c.Visible).ToArray();
            foreach (var c in _hiddenSiblings) c.Visible = false;

            // Save layout and fill
            _oldDock = this.Dock;
            _oldBounds = this.Bounds;

            this.Dock = DockStyle.Fill;
            _isFullInsideForm = true;

            this.Focus();
            btnFullscreen.BackgroundImage = _icoExitFullscreen;

            if (btnFullscreen.BackgroundImage == null)
                btnFullscreen.Text = "⤡";
            
        }

        private void ExitFullscreenInsideForm()
        {
            var parent = this.Parent;
            if (parent == null) return;

            // Restore siblings
            if (_hiddenSiblings != null)
            {
                foreach (var c in _hiddenSiblings) c.Visible = true;
                _hiddenSiblings = null;
            }

            // Restore layout
            this.Dock = _oldDock;
            this.Bounds = _oldBounds;

            _isFullInsideForm = false;
            this.Focus();
            btnFullscreen.BackgroundImage = _icoFullscreen;

            if (btnFullscreen.BackgroundImage == null)
                btnFullscreen.Text = "⤢";
        }

        /// <summary>
        /// Fullscreen in separate borderless form (true fullscreen).
        /// </summary>
        private void EnterFullscreenSeparateForm()
        {
            var parent = this.Parent;
            if (parent == null) return;

            _oldParent = parent;
            _oldDock = this.Dock;
            _oldBounds = this.Bounds;

            _fsForm = new Form
            {
                BackColor = Color.Black,
                FormBorderStyle = FormBorderStyle.None,
                WindowState = FormWindowState.Maximized,
                //TopMost = true,
                StartPosition = FormStartPosition.Manual
            };

            // Optional: allow dragging the fullscreen window by mouse
            _fsForm.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(_fsForm.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
                }
            };

            // Move this control into the fullscreen form
            this.Parent = _fsForm;
            this.Dock = DockStyle.Fill;

            _fsForm.Show();
            _fsForm.Focus();
            this.Focus();

            _isFullscreen = true;
        }

        private void ExitFullscreenSeparateForm()
        {
            if (_fsForm == null || _oldParent == null) return;

            // Move back to original parent
            this.Parent = _oldParent;
            this.Dock = _oldDock;
            this.Bounds = _oldBounds;

            _fsForm.Close();
            _fsForm.Dispose();
            _fsForm = null;

            _isFullscreen = false;
            this.Focus();
        }

        private string FormatTime(TimeSpan t) =>
            (t.Hours > 0) ? $"{t.Hours:00}:{t.Minutes:00}:{t.Seconds:00}"
                          : $"{t.Minutes:00}:{t.Seconds:00}";
        
        
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                try { _uiTimer?.Stop(); _overlayTimer?.Stop(); } catch { }
                _uiTimer?.Dispose();
                _overlayTimer?.Dispose();

                _icoPlay?.Dispose();
                _icoPause?.Dispose();
                _icoReplay?.Dispose();
                _icoFullscreen?.Dispose();
                _icoVolume?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

