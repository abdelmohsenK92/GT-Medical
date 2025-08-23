using System.Drawing;
using System.Windows.Forms;
using LibVLCSharp.WinForms;

namespace GT_Medical.UI
{
    partial class VlcPlayerControl
    {
        private Panel root;
        private VideoView videoView;
        private Panel pnlControls;
        private Panel pnlLeft;
        private Panel pnlRight;
        private Panel pnlCenter;
        private Button btnPlayPause;
        private Button btnReplay;
        private Button btnFullscreen;
        private TrackBar tbSeek;
        private Label lblTime;
        private TrackBar tbVolume;
        private PictureBox picVolume;
        private Button btnCenter;

        private void InitializeComponent()
        {
            root = new Panel();
            tbVolume = new TrackBar();
            videoView = new VideoView();
            pnlControls = new Panel();
            pnlCenter = new Panel();
            tbSeek = new TrackBar();
            lblTime = new Label();
            pnlLeft = new Panel();
            btnPlayPause = new Button();
            btnReplay = new Button();
            pnlRight = new Panel();
            picVolume = new PictureBox();
            btnFullscreen = new Button();
            btnCenter = new Button();
            root.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)tbVolume).BeginInit();
            ((System.ComponentModel.ISupportInitialize)videoView).BeginInit();
            pnlControls.SuspendLayout();
            pnlCenter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)tbSeek).BeginInit();
            pnlLeft.SuspendLayout();
            pnlRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picVolume).BeginInit();
            SuspendLayout();
            // 
            // root
            // 
            root.BackColor = Color.Black;
            root.Controls.Add(tbVolume);
            root.Controls.Add(videoView);
            root.Controls.Add(pnlControls);
            root.Controls.Add(btnCenter);
            root.Dock = DockStyle.Fill;
            root.Location = new Point(0, 0);
            root.Name = "root";
            root.Size = new Size(800, 450);
            root.TabIndex = 0;
            // 
            // tbVolume
            // 
            tbVolume.AutoSize = false;
            tbVolume.BackColor = Color.FromArgb(20, 28, 55);
            tbVolume.Location = new Point(707, 312);
            tbVolume.Maximum = 100;
            tbVolume.Name = "tbVolume";
            tbVolume.Orientation = Orientation.Vertical;
            tbVolume.Size = new Size(38, 96);
            tbVolume.TabIndex = 1;
            tbVolume.TickStyle = TickStyle.None;
            tbVolume.Value = 100;
            tbVolume.Visible = false;
            // 
            // videoView
            // 
            videoView.BackColor = Color.FromArgb(24, 24, 24);
            videoView.Dock = DockStyle.Fill;
            videoView.Location = new Point(0, 0);
            videoView.MediaPlayer = null;
            videoView.Name = "videoView";
            videoView.Size = new Size(800, 407);
            videoView.TabIndex = 0;
            videoView.Text = "videoView";
            // 
            // pnlControls
            // 
            pnlControls.BackColor = Color.FromArgb(20, 28, 55);
            pnlControls.BorderStyle = BorderStyle.FixedSingle;
            pnlControls.Controls.Add(pnlCenter);
            pnlControls.Controls.Add(pnlLeft);
            pnlControls.Controls.Add(pnlRight);
            pnlControls.Dock = DockStyle.Bottom;
            pnlControls.Location = new Point(0, 407);
            pnlControls.Name = "pnlControls";
            pnlControls.Size = new Size(800, 43);
            pnlControls.TabIndex = 1;
            // 
            // pnlCenter
            // 
            pnlCenter.AutoSize = true;
            pnlCenter.Controls.Add(tbSeek);
            pnlCenter.Controls.Add(lblTime);
            pnlCenter.Dock = DockStyle.Fill;
            pnlCenter.Location = new Point(105, 0);
            pnlCenter.Name = "pnlCenter";
            pnlCenter.Padding = new Padding(8, 10, 8, 10);
            pnlCenter.Size = new Size(597, 41);
            pnlCenter.TabIndex = 1;
            // 
            // tbSeek
            // 
            tbSeek.BackColor = Color.FromArgb(20, 28, 55);
            tbSeek.Dock = DockStyle.Fill;
            tbSeek.Location = new Point(8, 10);
            tbSeek.Maximum = 1000;
            tbSeek.Name = "tbSeek";
            tbSeek.Size = new Size(508, 21);
            tbSeek.TabIndex = 0;
            tbSeek.TickStyle = TickStyle.None;
            // 
            // lblTime
            // 
            lblTime.Dock = DockStyle.Right;
            lblTime.ForeColor = Color.Gainsboro;
            lblTime.Location = new Point(516, 10);
            lblTime.Name = "lblTime";
            lblTime.Size = new Size(73, 21);
            lblTime.TabIndex = 0;
            lblTime.Text = "00:00 / 00:00";
            lblTime.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlLeft
            // 
            pnlLeft.AutoSize = true;
            pnlLeft.Controls.Add(btnPlayPause);
            pnlLeft.Controls.Add(btnReplay);
            pnlLeft.Dock = DockStyle.Left;
            pnlLeft.Location = new Point(0, 0);
            pnlLeft.Name = "pnlLeft";
            pnlLeft.Padding = new Padding(8);
            pnlLeft.Size = new Size(105, 41);
            pnlLeft.TabIndex = 0;
            // 
            // btnPlayPause
            // 
            btnPlayPause.BackColor = Color.Transparent;
            btnPlayPause.BackgroundImage = Properties.Resources.play_button;
            btnPlayPause.BackgroundImageLayout = ImageLayout.Zoom;
            btnPlayPause.FlatAppearance.BorderSize = 0;
            btnPlayPause.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnPlayPause.FlatAppearance.MouseOverBackColor = Color.FromArgb(110, 20, 20, 70);
            btnPlayPause.FlatStyle = FlatStyle.Flat;
            btnPlayPause.ForeColor = Color.White;
            btnPlayPause.Location = new Point(6, 3);
            btnPlayPause.Name = "btnPlayPause";
            btnPlayPause.Size = new Size(38, 31);
            btnPlayPause.TabIndex = 0;
            btnPlayPause.UseVisualStyleBackColor = false;
            // 
            // btnReplay
            // 
            btnReplay.BackColor = Color.Transparent;
            btnReplay.BackgroundImage = Properties.Resources.replay__1_;
            btnReplay.BackgroundImageLayout = ImageLayout.Zoom;
            btnReplay.FlatAppearance.BorderSize = 0;
            btnReplay.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnReplay.FlatAppearance.MouseOverBackColor = Color.FromArgb(110, 20, 20, 70);
            btnReplay.FlatStyle = FlatStyle.Flat;
            btnReplay.ForeColor = Color.White;
            btnReplay.Location = new Point(56, 3);
            btnReplay.Name = "btnReplay";
            btnReplay.Size = new Size(38, 31);
            btnReplay.TabIndex = 1;
            btnReplay.UseVisualStyleBackColor = false;
            // 
            // pnlRight
            // 
            pnlRight.AutoSize = true;
            pnlRight.Controls.Add(picVolume);
            pnlRight.Controls.Add(btnFullscreen);
            pnlRight.Dock = DockStyle.Right;
            pnlRight.Location = new Point(702, 0);
            pnlRight.Name = "pnlRight";
            pnlRight.Padding = new Padding(8);
            pnlRight.Size = new Size(96, 41);
            pnlRight.TabIndex = 2;
            // 
            // picVolume
            // 
            picVolume.BackColor = Color.Transparent;
            picVolume.BackgroundImage = Properties.Resources.medium_volume;
            picVolume.BackgroundImageLayout = ImageLayout.Zoom;
            picVolume.Location = new Point(3, 3);
            picVolume.Name = "picVolume";
            picVolume.Size = new Size(38, 31);
            picVolume.SizeMode = PictureBoxSizeMode.CenterImage;
            picVolume.TabIndex = 3;
            picVolume.TabStop = false;
            picVolume.Visible = false;
            // 
            // btnFullscreen
            // 
            btnFullscreen.BackColor = Color.Transparent;
            btnFullscreen.BackgroundImage = Properties.Resources.expand;
            btnFullscreen.BackgroundImageLayout = ImageLayout.Zoom;
            btnFullscreen.FlatAppearance.BorderSize = 0;
            btnFullscreen.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnFullscreen.FlatAppearance.MouseOverBackColor = Color.FromArgb(110, 20, 20, 70);
            btnFullscreen.FlatStyle = FlatStyle.Flat;
            btnFullscreen.ForeColor = Color.White;
            btnFullscreen.Location = new Point(47, 5);
            btnFullscreen.Name = "btnFullscreen";
            btnFullscreen.Size = new Size(38, 29);
            btnFullscreen.TabIndex = 2;
            btnFullscreen.UseVisualStyleBackColor = false;
            // 
            // btnCenter
            // 
            btnCenter.Anchor = AnchorStyles.None;
            btnCenter.BackColor = Color.Transparent;
            btnCenter.BackgroundImage = Properties.Resources.pause_button;
            btnCenter.BackgroundImageLayout = ImageLayout.Zoom;
            btnCenter.FlatAppearance.BorderSize = 0;
            btnCenter.FlatStyle = FlatStyle.Flat;
            btnCenter.Font = new Font("Segoe UI", 30F);
            btnCenter.Location = new Point(370, 195);
            btnCenter.Name = "btnCenter";
            btnCenter.Size = new Size(60, 60);
            btnCenter.TabIndex = 3;
            btnCenter.UseVisualStyleBackColor = false;
            // 
            // VlcPlayerControl
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(24, 24, 24);
            Controls.Add(root);
            Name = "VlcPlayerControl";
            Size = new Size(800, 450);
            root.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)tbVolume).EndInit();
            ((System.ComponentModel.ISupportInitialize)videoView).EndInit();
            pnlControls.ResumeLayout(false);
            pnlControls.PerformLayout();
            pnlCenter.ResumeLayout(false);
            pnlCenter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)tbSeek).EndInit();
            pnlLeft.ResumeLayout(false);
            pnlRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picVolume).EndInit();
            ResumeLayout(false);

        }
    }
}
