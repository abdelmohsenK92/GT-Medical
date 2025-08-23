using GT_Medical.Abstractions;
using GT_Medical.Helper.Extensions;
using GT_Medical.Infrastructure;
using LibVLCSharp.Shared;
using SharpCompress.Archives;
using SharpCompress.Common;
using System;
using System.ComponentModel;
using System.Formats.Tar;
using System.IO;
using System.Threading.Tasks;

namespace GT_Medical.Services
{
    /// <summary>
    /// Owns LibVLC + MediaPlayer. Binds MediaPlayer to the UI's VideoSurface (VideoView),
    /// reacts to UI events, and pushes playback state back to UI.
    /// </summary>
    public class VideoPlayer : CrossThreadInvoker
    {
        private readonly VideoDb _db;
        private readonly IVideoSurfaceUi _ui;
        private readonly Action<string,int> _toast;

        private LibVLC _libvlc;
        private MediaPlayer _mp;
        public VideoDb Data => _db;

        public MediaPlayer VlcMediaPlayer { get => _mp; set => _mp = value; }
        public VideoPlayer(ISynchronizeInvoke uiInvoker,
                           IVideoSurfaceUi uiSurface,
                           Action<string,int> toast = null) : base(uiInvoker)
        {
            _ui = uiSurface ?? throw new ArgumentNullException(nameof(uiSurface));
            _toast = toast;

            _db = new VideoDb(uiInvoker);
            _db.AttachUi(uiInvoker);

            // wire UI events -> backend actions
            _ui.TogglePlayPauseRequested += () => _mp?.SetPause(_mp?.IsPlaying == true);
            _ui.ReplayRequested += () => { if (_mp != null) { _mp.Stop(); _mp.Play(); } };
            _ui.VolumeChanged += v => { if (_mp != null) _mp.Volume = Math.Max(0, Math.Min(100, v)); PushState(); };
            _ui.SeekRequested += p =>
            {
                if (_mp != null && _mp.Length > 0)
                {
                    _mp.Position = Math.Max(0f, Math.Min(1f, p));
                    PushState();
                }
            };

        }

        /// <summary>
        /// Initialize LibVLC/MediaPlayer and bind to UI's VideoSurface; then init DB.
        /// </summary>
        public async Task InitializeAsync()
        {
            try
            {
                ExtractLibVLC();
                Core.Initialize();
                _libvlc = new LibVLC("--no-video-title-show", "--quiet", "--network-caching=2000");
                _mp = new MediaPlayer(_libvlc);

                // bind to UI surface
                RunOnUi(() =>
                {
                    _ui.VideoSurface.MediaPlayer = VlcMediaPlayer;
                    _ui.NotifyFinishInitialization();
                });

                // push progress back to UI
                _mp.TimeChanged += (s, e) => PushState();
                _mp.LengthChanged += (s, e) => PushState();
                _mp.Volume = 70;
                PushState();

                // DB (scan/load/merge)
                await _db.InitializeAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ExtractLibVLC()
        {
            string libvlcDir = AppDomain.CurrentDomain.BaseDirectory + "\\libvlc";
            if (!Directory.Exists(libvlcDir))
            {
                var archiveFile = libvlcDir + ".rar";
                if (File.Exists(libvlcDir))
                {
                    using (var archive = ArchiveFactory.Open(archiveFile))
                    {
                        foreach (var entry in archive.Entries)
                        {
                            if (!entry.IsDirectory)
                            {
                                Console.WriteLine($"Extracting: {entry.Key}");
                                entry.WriteToDirectory(AppDomain.CurrentDomain.BaseDirectory, new ExtractionOptions()
                                {
                                    ExtractFullPath = true,   // keep folder structure
                                    Overwrite = true          // overwrite if file exists
                                });
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Play by barcode: local if exists, otherwise stream RemoteUrl.
        /// </summary>
        public async Task PlayByBarcodeAsync(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode)) return;

            var item = _db.GetByBarcode(barcode);
            if (item == null) { _toast("Barcode not found.",3000); return; }
                
            // Local first
            if (!string.IsNullOrWhiteSpace(item.LocalPath) && File.Exists(item.LocalPath))
            {
                PlayLocal(item.LocalPath);
                var duration = Convert.ToInt32(_mp.Length / 2);
                _toast(item.Description ?? item.Name ?? Path.GetFileName(item.LocalPath),
                    Convert.ToInt32(_mp.Length/2));
                //await _db.UpdateStatusAsync(barcode, "Idle");
                return;
            }

            // Fallback: remote
            if (!string.IsNullOrWhiteSpace(item.RemoteUrl))
            {
                PlayRemote(item.RemoteUrl);
                _toast(item.Name ?? "Streaming…",3000);
                await _db.UpdateStatusAsync(barcode, "Streaming");
                return;
            }

            await _db.UpdateStatusAsync(barcode, "Missing");
            _toast("Video is missing (no local file, no remote URL).", 3000);
        }

        public void PlayLocal(string path)
        {
            if (_mp == null) return;
            _mp.Stop();
            using var media = new Media(_libvlc, new Uri(Path.GetFullPath(path)));
            media.AddOption(":file-caching=1500");
            _mp.Play(media);
            
            PushState();
        }

        public void PlayRemote(string url)
        {
            if (_mp == null) return;
            _mp.Stop();
            using var media = new Media(_libvlc, new Uri(url));
            media.AddOption(":network-caching=3000");
            _mp.Play(media);
            PushState();
        }

        public void Stop()
        {
            _mp?.Stop();
            PushState();
        }

        public void SetVolume(int v)
        {
            if (_mp == null) return;
            _mp.Volume = Math.Max(0, Math.Min(100, v));
            PushState();
        }

        public void SeekPercent(float p01)
        {
            if (_mp == null || _mp.Length <= 0) return;
            _mp.Position = Math.Max(0f, Math.Min(1f, p01));
            PushState();
        }

        private void PushState()
        {
            if (_mp == null) return;
            var isPlaying = _mp.IsPlaying;
            var t = _mp.Time;
            var len = _mp.Length;
            var vol = _mp.Volume;
            var pos = _mp.Position;

            _ui.SetPlaybackState(isPlaying, t, len, vol, pos);
        }

        public void Dispose()
        {
            try { _mp?.Stop(); } catch { }
            _mp?.Dispose();
            _libvlc?.Dispose();
        }

    }
    public enum FullscreenMode
    {
        InsideForm,     // Fill inside current form and hide siblings
        SeparateForm    // Move control into borderless fullscreen form
    }
}
