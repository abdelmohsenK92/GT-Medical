namespace GT_Medical.UI
{
    partial class FrmLoading
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
            pictureBox1 = new PictureBox();
            LblSending = new Label();
            panel1 = new Panel();
            PnlProgress = new Panel();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel1.SuspendLayout();
            PnlProgress.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImageLayout = ImageLayout.Center;
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Image = Properties.Resources.DualLoading;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Margin = new Padding(4, 3, 4, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(230, 200);
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // LblSending
            // 
            LblSending.BackColor = Color.White;
            LblSending.Dock = DockStyle.Fill;
            LblSending.Font = new Font("Calibri", 20F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LblSending.ForeColor = Color.FromArgb(20, 28, 36);
            LblSending.Location = new Point(0, 0);
            LblSending.Margin = new Padding(4, 0, 4, 0);
            LblSending.Name = "LblSending";
            LblSending.Size = new Size(230, 42);
            LblSending.TabIndex = 1;
            LblSending.Text = "جاري التهيئة ...";
            LblSending.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            panel1.Controls.Add(pictureBox1);
            panel1.Controls.Add(PnlProgress);
            panel1.Location = new Point(351, 13);
            panel1.Margin = new Padding(4, 3, 4, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(230, 242);
            panel1.TabIndex = 2;
            // 
            // PnlProgress
            // 
            PnlProgress.Controls.Add(LblSending);
            PnlProgress.Dock = DockStyle.Bottom;
            PnlProgress.Location = new Point(0, 200);
            PnlProgress.Margin = new Padding(4, 3, 4, 3);
            PnlProgress.Name = "PnlProgress";
            PnlProgress.Size = new Size(230, 42);
            PnlProgress.TabIndex = 3;
            // 
            // FrmLoading
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(20, 28, 36);
            ClientSize = new Size(933, 269);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4, 3, 4, 3);
            Name = "FrmLoading";
            Opacity = 0.6D;
            RightToLeft = RightToLeft.Yes;
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FrmLoading";
            Load += FrmProgress_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel1.ResumeLayout(false);
            PnlProgress.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label LblSending;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel PnlProgress;
    }
}