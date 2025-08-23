namespace GT_Medical.UI
{
    partial class FrmVideoPlayer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            PnlSplitter = new SplitContainer();
            TxtBarcode = new TextBox();
            VLCPlayer = new VlcPlayerControl();
            ((System.ComponentModel.ISupportInitialize)PnlSplitter).BeginInit();
            PnlSplitter.Panel1.SuspendLayout();
            PnlSplitter.Panel2.SuspendLayout();
            PnlSplitter.SuspendLayout();
            SuspendLayout();
            // 
            // PnlSplitter
            // 
            PnlSplitter.Dock = DockStyle.Fill;
            PnlSplitter.FixedPanel = FixedPanel.Panel1;
            PnlSplitter.IsSplitterFixed = true;
            PnlSplitter.Location = new Point(0, 35);
            PnlSplitter.Name = "PnlSplitter";
            PnlSplitter.Orientation = Orientation.Horizontal;
            // 
            // PnlSplitter.Panel1
            // 
            PnlSplitter.Panel1.BackColor = Color.FromArgb(20, 28, 36);
            PnlSplitter.Panel1.Controls.Add(TxtBarcode);
            PnlSplitter.Panel1.RightToLeft = RightToLeft.Yes;
            // 
            // PnlSplitter.Panel2
            // 
            PnlSplitter.Panel2.Controls.Add(VLCPlayer);
            PnlSplitter.Panel2.RightToLeft = RightToLeft.Yes;
            PnlSplitter.Size = new Size(967, 457);
            PnlSplitter.SplitterDistance = 60;
            PnlSplitter.TabIndex = 2;
            // 
            // TxtBarcode
            // 
            TxtBarcode.Anchor = AnchorStyles.Top;
            TxtBarcode.BackColor = Color.FromArgb(20, 28, 55);
            TxtBarcode.Font = new Font("Calibri", 20F);
            TxtBarcode.ForeColor = Color.White;
            TxtBarcode.Location = new Point(254, 10);
            TxtBarcode.Name = "TxtBarcode";
            TxtBarcode.Size = new Size(458, 40);
            TxtBarcode.TabIndex = 2;
            TxtBarcode.TextAlign = HorizontalAlignment.Center;
            TxtBarcode.KeyDown += TxtBarcode_KeyDown;
            // 
            // VLCPlayer
            // 
            VLCPlayer.BackColor = Color.Black;
            VLCPlayer.Dock = DockStyle.Fill;
            VLCPlayer.Location = new Point(0, 0);
            VLCPlayer.Mode = Services.FullscreenMode.SeparateForm;
            VLCPlayer.Name = "VLCPlayer";
            VLCPlayer.Size = new Size(967, 393);
            VLCPlayer.TabIndex = 1;
            // 
            // FrmVideoPlayer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(967, 492);
            Controls.Add(PnlSplitter);
            Name = "FrmVideoPlayer";
            Text = "GT-Medical Video Hub";
            WindowState = FormWindowState.Maximized;
            KeyDown += FrmVideoPlayer_KeyDown;
            Controls.SetChildIndex(PnlSplitter, 0);
            PnlSplitter.Panel1.ResumeLayout(false);
            PnlSplitter.Panel1.PerformLayout();
            PnlSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)PnlSplitter).EndInit();
            PnlSplitter.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer PnlSplitter;
        private TextBox TxtBarcode;
        private VlcPlayerControl VLCPlayer;
    }
}