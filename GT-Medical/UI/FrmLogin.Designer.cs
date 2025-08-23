namespace GT_Medical.UI
{
    partial class FrmLogin
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
            PnlFields = new Panel();
            PnlPassword = new Panel();
            TxtPass = new TextBox();
            pictureBox2 = new PictureBox();
            PnlUserName = new Panel();
            TxtUser = new TextBox();
            pictureBox1 = new PictureBox();
            BtnLogin = new Button();
            PnlFields.SuspendLayout();
            PnlPassword.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            PnlUserName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();            
            // 
            // PnlFields
            // 
            PnlFields.Controls.Add(PnlPassword);
            PnlFields.Controls.Add(PnlUserName);
            PnlFields.Controls.Add(BtnLogin);
            PnlFields.Dock = DockStyle.Fill;
            PnlFields.Font = new Font("Calibri", 12F, FontStyle.Bold);
            PnlFields.Location = new Point(0, 35);
            PnlFields.Name = "PnlFields";
            PnlFields.Size = new Size(623, 251);
            PnlFields.TabIndex = 1;
            // 
            // PnlPassword
            // 
            PnlPassword.AutoSize = true;
            PnlPassword.Controls.Add(TxtPass);
            PnlPassword.Controls.Add(pictureBox2);
            PnlPassword.Location = new Point(86, 122);
            PnlPassword.Name = "PnlPassword";
            PnlPassword.Size = new Size(450, 42);
            PnlPassword.TabIndex = 6;
            // 
            // TxtPass
            // 
            TxtPass.BackColor = Color.FromArgb(20, 28, 55);
            TxtPass.Dock = DockStyle.Fill;
            TxtPass.Font = new Font("Calibri", 20F, FontStyle.Bold);
            TxtPass.ForeColor = Color.White;
            TxtPass.Location = new Point(0, 0);
            TxtPass.MaxLength = 100;
            TxtPass.Name = "TxtPass";
            TxtPass.Size = new Size(416, 40);
            TxtPass.TabIndex = 1;
            TxtPass.TextAlign = HorizontalAlignment.Center;
            TxtPass.UseSystemPasswordChar = true;
            // 
            // pictureBox2
            // 
            pictureBox2.BackgroundImage = Properties.Resources.padlock;
            pictureBox2.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox2.Dock = DockStyle.Right;
            pictureBox2.Location = new Point(416, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(34, 42);
            pictureBox2.TabIndex = 6;
            pictureBox2.TabStop = false;
            // 
            // PnlUserName
            // 
            PnlUserName.AutoSize = true;
            PnlUserName.Controls.Add(TxtUser);
            PnlUserName.Controls.Add(pictureBox1);
            PnlUserName.Location = new Point(86, 63);
            PnlUserName.Name = "PnlUserName";
            PnlUserName.Size = new Size(450, 42);
            PnlUserName.TabIndex = 5;
            // 
            // TxtUser
            // 
            TxtUser.BackColor = Color.FromArgb(20, 28, 55);
            TxtUser.Dock = DockStyle.Fill;
            TxtUser.Font = new Font("Calibri", 20F, FontStyle.Bold);
            TxtUser.ForeColor = Color.White;
            TxtUser.Location = new Point(0, 0);
            TxtUser.MaxLength = 100;
            TxtUser.Name = "TxtUser";
            TxtUser.Size = new Size(416, 40);
            TxtUser.TabIndex = 0;
            TxtUser.TextAlign = HorizontalAlignment.Center;
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImage = Properties.Resources.user__1_;
            pictureBox1.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox1.Dock = DockStyle.Right;
            pictureBox1.Location = new Point(416, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(34, 42);
            pictureBox1.TabIndex = 6;
            pictureBox1.TabStop = false;
            // 
            // BtnLogin
            // 
            BtnLogin.BackColor = Color.FromArgb(0, 192, 192);
            BtnLogin.FlatAppearance.BorderSize = 0;
            BtnLogin.FlatStyle = FlatStyle.Flat;
            BtnLogin.Font = new Font("Calibri", 15F, FontStyle.Bold);
            BtnLogin.ForeColor = Color.FromArgb(20, 28, 55);
            BtnLogin.Location = new Point(196, 185);
            BtnLogin.Name = "BtnLogin";
            BtnLogin.Size = new Size(231, 45);
            BtnLogin.TabIndex = 4;
            BtnLogin.Text = "تسجيل الدخول";
            BtnLogin.UseVisualStyleBackColor = false;
            BtnLogin.Click += BtnLogin_Click;
            // 
            // FrmLogin
            // 
            AcceptButton = BtnLogin;
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(623, 286);
            Controls.Add(PnlFields);
            Font = new Font("Segoe UI", 12F);
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmLogin";
            Text = "Login";
            Controls.SetChildIndex(PnlFields, 0);
            PnlFields.ResumeLayout(false);
            PnlFields.PerformLayout();
            PnlPassword.ResumeLayout(false);
            PnlPassword.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            PnlUserName.ResumeLayout(false);
            PnlUserName.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Panel PnlFields;
        private System.Windows.Forms.TextBox TxtUser;
        private System.Windows.Forms.TextBox TxtPass;
        private System.Windows.Forms.Button BtnLogin;
        private Panel PnlUserName;
        private PictureBox pictureBox1;
        private Panel PnlPassword;
        private PictureBox pictureBox2;
    }
}