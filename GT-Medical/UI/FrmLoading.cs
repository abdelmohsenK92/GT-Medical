using GT_Medical.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace GT_Medical.UI
{
    public partial class FrmLoading : Form, ITransientService
    {
        private readonly Form _owner;
        private readonly Timer _timerDots;

        public FrmLoading(Form owner)
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            _owner = owner;
            _timerDots = new Timer()
            {
                Interval = 1000,
            };
            _timerDots.Tick += (s, e) =>
            {
                var dots = LblSending.Text.ToArray();
                var dotsCount = dots.Count(c => c == '.');
                if (dotsCount == 3)
                    LblSending.Text = LblSending.Text.Replace("...", ".");
                else
                    LblSending.Text += ".";
            };
            this.HandleCreated += (s, e) => _timerDots.Start();
        }
    
        private void FrmProgress_Load(object sender, EventArgs e)
        {
            this.Size = _owner.Size;
            this.Left = _owner.Left;
            this.Top = _owner.Top;
            panel1.Top = (this.Height - panel1.Height) / 2;
            panel1.Left = (this.Width - panel1.Width) / 2;
            BringToFront();

        }
    }
}
