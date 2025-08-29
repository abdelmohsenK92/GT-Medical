using GT_Medical.Abstractions;
using GT_Medical.Helper;
using GT_Medical.Services;
namespace GT_Medical.UI;
public partial class FrmVideoPlayer : BaseForm, ISingletonService
{
    private FrmLoading _loading;
    VideoPlayer _player;
    private CrossThreadInvoker _invoker;

    public FrmVideoPlayer() : base()
    {
        InitializeComponent();
        _invoker = new CrossThreadInvoker(this);

        ExitClicked += (s, e) => Application.Exit();
        _player = new VideoPlayer(this, VLCPlayer, ShowTip);

        TxtBarcode.Padding = new Padding(2, 4, 2, 4);
        _loading = new FrmLoading(this);
        this.KeyPreview = true;
        PnlSplitter.Visible = false;
        Shown += async (s, e) =>
        {
            SetCueBanner(TxtBarcode, "أدخل الباركود لعرض الفيديو");
            TxtBarcode.Select();
            await InitializeAsync();
            _loading.ShowDialog(this);
        };
    }
    public FrmVideoPlayer(string videoPath) : this()
    {
        PnlSplitter.Panel1Collapsed = true;
        Shown += (s, e) =>
        {
            _player.PlayLocal(videoPath);
        };
    }
    public Task InitializeAsync()
    {
        return Task.Factory.StartNew(async () =>
        {
            try
            {
                var max = 500 * 60;  
                var cur = 0;
                while (!_loading.IsHandleCreated || cur > max)
                {
                    await Task.Delay(500);
                    cur += 500;
                }
                await _player.InitializeAsync();

                _invoker.RunOnUi(() =>
                {
                    // bind to UI surface
                    //VLCPlayer.VideoSurface.MediaPlayer = _player.VlcMediaPlayer;
                    _loading.Close();
                    PnlSplitter.Visible = true;
                });
            }
            catch (Exception ex)
            {
                _loading.Close();
                MessageBox.Show("حدث خطأ أثناء التهيئة: " + ex.ToString(), "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        });
    }
    private void BtnExit_Click(object sender, EventArgs e)
    {
        Application.Exit();
    }
    private async void TxtBarcode_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.Handled = true;
            e.SuppressKeyPress = true;

            var barcode = (TxtBarcode.Text ?? "").Trim();

            if (string.IsNullOrWhiteSpace(barcode))
            {
                // فيدباك بصري بسيط لو فاضي
                await ShakeAsync(TxtBarcode);
                return;
            }

            // نقطة الدخول: سواء ماسح بعت Enter أو المستخدم ضغط Enter يدويًا
            await _player.PlayByBarcodeAsync(barcode);

            // جهّز للماسحة التالية (معظم الماسحات تكتب فوق التحديد)
            TxtBarcode.SelectAll();
        }
    }
 

    private async Task ShakeAsync(Control ctrl)
    {
        var orig = ctrl.Location;
        for (int i = 0; i < 8; i++)
        {
            ctrl.Left = orig.X + (i % 2 == 0 ? -4 : 4);
            await Task.Delay(15);
        }
        ctrl.Left = orig.X;
    }

    private void FrmVideoPlayer_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape)
        {
            this.Close();
        }
        else if (e.KeyCode == Keys.F1)
        {
            var login = new FrmLogin();
            if (login.ShowDialog() == DialogResult.OK)
                new FrmItems(_player.Data).ShowDialog(this);

        }
    }
}

