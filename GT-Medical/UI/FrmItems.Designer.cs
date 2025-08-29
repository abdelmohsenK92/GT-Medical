namespace GT_Medical.UI
{
    partial class FrmItems
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DGVData = new DataGridView();
            panel1 = new Panel();
            label1 = new Label();
            panel2 = new Panel();
            TxtPath = new TextBox();
            BtnBrowse = new Button();
            DlgFolderBrowser = new FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)DGVData).BeginInit();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // DGVData
            // 
            DGVData.AllowUserToDeleteRows = false;
            DGVData.BackgroundColor = Color.FromArgb(20, 28, 55);
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(24, 24, 24);
            dataGridViewCellStyle1.Font = new Font("Calibri", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            DGVData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            DGVData.ColumnHeadersHeight = 30;
            DGVData.Dock = DockStyle.Fill;
            DGVData.GridColor = Color.FromArgb(50, 70, 90);
            DGVData.Location = new Point(0, 35);
            DGVData.Name = "DGVData";
            DGVData.RowTemplate.DividerHeight = 3;
            DGVData.RowTemplate.Height = 30;
            DGVData.RowTemplate.Resizable = DataGridViewTriState.True;
            DGVData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DGVData.Size = new Size(800, 374);
            DGVData.TabIndex = 0;
            DGVData.CellBeginEdit += DGVData_CellBeginEdit;
            DGVData.CellContentClick += DGVData_CellContentClick;
            DGVData.CellValueChanged += DGVData_CellValueChanged;
            // 
            // panel1
            // 
            panel1.AutoSize = true;
            panel1.Controls.Add(label1);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 409);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 41);
            panel1.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Calibri", 16F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(627, 8);
            label1.Name = "label1";
            label1.Size = new Size(108, 27);
            label1.TabIndex = 2;
            label1.Text = "مسار الفيديو";
            // 
            // panel2
            // 
            panel2.AutoSize = true;
            panel2.Controls.Add(TxtPath);
            panel2.Controls.Add(BtnBrowse);
            panel2.Location = new Point(66, 4);
            panel2.Name = "panel2";
            panel2.Size = new Size(555, 34);
            panel2.TabIndex = 1;
            // 
            // TxtPath
            // 
            TxtPath.BackColor = Color.FromArgb(24, 24, 24);
            TxtPath.Dock = DockStyle.Fill;
            TxtPath.Font = new Font("Segoe UI", 14F);
            TxtPath.ForeColor = Color.White;
            TxtPath.Location = new Point(41, 0);
            TxtPath.Name = "TxtPath";
            TxtPath.Size = new Size(514, 32);
            TxtPath.TabIndex = 0;
            TxtPath.TextAlign = HorizontalAlignment.Center;
            // 
            // BtnBrowse
            // 
            BtnBrowse.BackColor = Color.Transparent;
            BtnBrowse.BackgroundImage = Properties.Resources.folder;
            BtnBrowse.BackgroundImageLayout = ImageLayout.Zoom;
            BtnBrowse.Dock = DockStyle.Left;
            BtnBrowse.FlatAppearance.BorderColor = Color.White;
            BtnBrowse.FlatAppearance.BorderSize = 3;
            BtnBrowse.FlatStyle = FlatStyle.Flat;
            BtnBrowse.Location = new Point(0, 0);
            BtnBrowse.Name = "BtnBrowse";
            BtnBrowse.Size = new Size(41, 34);
            BtnBrowse.TabIndex = 0;
            BtnBrowse.UseVisualStyleBackColor = false;
            BtnBrowse.Click += BtnBrowse_Click;
            // 
            // FrmItems
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(DGVData);
            Controls.Add(panel1);
            Name = "FrmItems";
            Text = "GT-Medical Items";
            WindowState = FormWindowState.Maximized;
            Controls.SetChildIndex(panel1, 0);
            Controls.SetChildIndex(DGVData, 0);
            ((System.ComponentModel.ISupportInitialize)DGVData).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView DGVData;
        private Panel panel1;
        private Panel panel2;
        private Button BtnBrowse;
        private TextBox TxtPath;
        private FolderBrowserDialog DlgFolderBrowser;
        private Label label1;
    }
}