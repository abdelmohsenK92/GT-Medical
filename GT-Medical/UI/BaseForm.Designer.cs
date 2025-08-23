namespace GT_Medical.UI
{
    partial class BaseForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BaseForm));
            PnlHeader = new Panel();
            LblTitle = new Label();
            BtnExit = new Button();
            PnlHeader.SuspendLayout();
            SuspendLayout();
            // 
            // PnlHeader
            // 
            PnlHeader.Controls.Add(LblTitle);
            PnlHeader.Controls.Add(BtnExit);
            PnlHeader.Dock = DockStyle.Top;
            PnlHeader.Location = new Point(0, 0);
            PnlHeader.Name = "PnlHeader";
            PnlHeader.Size = new Size(436, 35);
            PnlHeader.TabIndex = 1;
            // 
            // LblTitle
            // 
            LblTitle.BackColor = Color.Transparent;
            LblTitle.Font = new Font("Segoe UI", 16F);
            LblTitle.ForeColor = Color.White;
            LblTitle.Image = Properties.Resources.favicon_1;
            LblTitle.ImageAlign = ContentAlignment.MiddleRight;
            LblTitle.Location = new Point(0, 0);
            LblTitle.Name = "LblTitle";
            LblTitle.Size = new Size(240, 35);
            LblTitle.TabIndex = 2;
            LblTitle.Text = "GT-Medical Video Hub";
            LblTitle.TextAlign = ContentAlignment.MiddleLeft;
            LblTitle.UseCompatibleTextRendering = true;
            // 
            // BtnExit
            // 
            BtnExit.BackColor = Color.Red;
            BtnExit.Dock = DockStyle.Right;
            BtnExit.FlatAppearance.MouseDownBackColor = Color.Maroon;
            BtnExit.FlatAppearance.MouseOverBackColor = Color.FromArgb(192, 0, 0);
            BtnExit.FlatStyle = FlatStyle.Flat;
            BtnExit.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            BtnExit.ForeColor = Color.White;
            BtnExit.Location = new Point(406, 0);
            BtnExit.Name = "BtnExit";
            BtnExit.Size = new Size(30, 35);
            BtnExit.TabIndex = 4;
            BtnExit.Text = "X";
            BtnExit.UseVisualStyleBackColor = false;
            // 
            // BaseForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(20, 28, 36);
            ClientSize = new Size(436, 214);
            Controls.Add(PnlHeader);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "BaseForm";
            RightToLeft = RightToLeft.Yes;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "BaseForm";
            PnlHeader.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel PnlHeader;
        private Button BtnExit;
        private Label LblTitle;
    }
}