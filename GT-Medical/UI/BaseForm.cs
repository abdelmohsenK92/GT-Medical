using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;


namespace GT_Medical.UI;

public partial class BaseForm : Form
{
    public event EventHandler ExitClicked;
    private float headerOpacity = 0f;
    public BaseForm()
    {
        if (DesignMode)
            return;
        InitializeComponent();
        BtnExit.Click += (s, e) =>
        {
            this.Close();
            ExitClicked?.Invoke(s, e);
        };
        var fadeTimer = new Timer { Interval = 15 };
        fadeTimer.Tick += (s, e) =>
        {
            headerOpacity += 0.05f;
            if (headerOpacity >= 1f) { headerOpacity = 1f; fadeTimer.Stop(); }
            PnlHeader.Invalidate();
        };
        fadeTimer.Start();

        PnlHeader.Paint += (s, e) =>
        {
            using var br = new System.Drawing.Drawing2D.LinearGradientBrush(PnlHeader.ClientRectangle,
                   Color.FromArgb((int)(255 * headerOpacity), 15, 32, 39),
                   Color.FromArgb((int)(255 * headerOpacity), 32, 58, 67),
                   0f);
            e.Graphics.FillRectangle(br, PnlHeader.ClientRectangle);
        };
        PnlHeader.MouseDown += (s, e) => OnMouseDown(e);
        LblTitle.MouseDown += (s, e) => OnMouseDown(e);

        PnlHeader.MouseDoubleClick +=  (s, e) =>
        {
            if (this.WindowState == FormWindowState.Normal)
                this.WindowState = FormWindowState.Maximized;
            else
                this.WindowState = FormWindowState.Normal;

        };
        LblTitle.MouseDoubleClick += (s, e) =>
        {
            if (this.WindowState == FormWindowState.Normal)
                this.WindowState = FormWindowState.Maximized;
            else
                this.WindowState = FormWindowState.Normal;

        };
        this.DoubleBuffered = true;
    }

    protected void SetTitle(string title)
    {
        LblTitle.Text = title;
    }
    protected void ShowTip(string text,int duration = 5000)
    {
        if (duration <= 0)
            duration = 5000;
        var tip = new ToolTip { IsBalloon = true, ToolTipTitle = "Info" };
        tip.Show(text, this, this.Width - 320, 10, duration);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        if (e.Button == MouseButtons.Left)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
        }
    }
    protected override void WndProc(ref Message m)
    {
        const int WM_NCHITTEST = 0x84;
        const int HTCLIENT = 1;
        const int HTLEFT = 10;
        const int HTRIGHT = 11;
        const int HTTOP = 12;
        const int HTTOPLEFT = 13;
        const int HTTOPRIGHT = 14;
        const int HTBOTTOM = 15;
        const int HTBOTTOMLEFT = 16;
        const int HTBOTTOMRIGHT = 17;

        if (m.Msg == WM_NCHITTEST)
        {
            base.WndProc(ref m);

            if ((int)m.Result == HTCLIENT)
            {
                int grip = 10;
                var pt = this.PointToClient(Cursor.Position);

                if (pt.X <= grip && pt.Y <= grip) m.Result = (IntPtr)HTTOPLEFT;
                else if (pt.X >= this.ClientSize.Width - grip && pt.Y <= grip) m.Result = (IntPtr)HTTOPRIGHT;
                else if (pt.X <= grip && pt.Y >= this.ClientSize.Height - grip) m.Result = (IntPtr)HTBOTTOMLEFT;
                else if (pt.X >= this.ClientSize.Width - grip && pt.Y >= this.ClientSize.Height - grip) m.Result = (IntPtr)HTBOTTOMRIGHT;
                else if (pt.X <= grip) m.Result = (IntPtr)HTLEFT;
                else if (pt.X >= this.ClientSize.Width - grip) m.Result = (IntPtr)HTRIGHT;
                else if (pt.Y <= grip) m.Result = (IntPtr)HTTOP;
                else if (pt.Y >= this.ClientSize.Height - grip) m.Result = (IntPtr)HTBOTTOM;
            }
            return;
        }
        base.WndProc(ref m);
    }


    protected static void SetCueBanner(TextBox tb, string text, bool showWhenFocused = true)
    {
        // wParam = 1 => يظهر حتى مع الفوكس (سلوك لطيف للماسحات)
        SendMessage(tb.Handle, EM_SETCUEBANNER, showWhenFocused ? 1 : 0, text);
    }

    // Win32 constants
    private const int WM_NCLBUTTONDOWN = 0xA1;
    private const int HTCAPTION = 0x2;
    protected const int EM_SETCUEBANNER = 0x1501;

    [DllImport("user32.dll")]
    private static extern bool ReleaseCapture();
    [DllImport("user32.dll")]
    private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);


    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    protected static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);
    
}

