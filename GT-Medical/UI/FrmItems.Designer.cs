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
            PnlSearch = new Panel();
            TxtSearch = new TextBox();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)DGVData).BeginInit();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            PnlSearch.SuspendLayout();
            SuspendLayout();
            // 
            // DGVData
            // 
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
            DGVData.Location = new Point(0, 74);
            DGVData.Name = "DGVData";
            DGVData.RowTemplate.DividerHeight = 3;
            DGVData.RowTemplate.Height = 30;
            DGVData.RowTemplate.Resizable = DataGridViewTriState.True;
            DGVData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DGVData.Size = new Size(1057, 337);
            DGVData.TabIndex = 0;
            DGVData.CellBeginEdit += DGVData_CellBeginEdit;
            DGVData.CellContentClick += DGVData_CellContentClick;
            DGVData.CellValueChanged += DGVData_CellValueChanged;
            DGVData.UserDeletingRow += DGVData_UserDeletingRow;
            // 
            // panel1
            // 
            panel1.AutoSize = true;
            panel1.Controls.Add(label1);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 411);
            panel1.Name = "panel1";
            panel1.Size = new Size(1057, 39);
            panel1.TabIndex = 2;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top;
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Calibri", 16F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(866, 6);
            label1.Name = "label1";
            label1.Size = new Size(108, 27);
            label1.TabIndex = 2;
            label1.Text = "مسار الفيديو";
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Top;
            panel2.AutoSize = true;
            panel2.Controls.Add(TxtPath);
            panel2.Controls.Add(BtnBrowse);
            panel2.Location = new Point(82, 2);
            panel2.Name = "panel2";
            panel2.Size = new Size(778, 34);
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
            TxtPath.Size = new Size(737, 32);
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
            // PnlSearch
            // 
            PnlSearch.AutoSize = true;
            PnlSearch.Controls.Add(TxtSearch);
            PnlSearch.Controls.Add(label2);
            PnlSearch.Dock = DockStyle.Top;
            PnlSearch.Location = new Point(0, 35);
            PnlSearch.Name = "PnlSearch";
            PnlSearch.Size = new Size(1057, 39);
            PnlSearch.TabIndex = 3;
            // 
            // TxtSearch
            // 
            TxtSearch.Anchor = AnchorStyles.Top;
            TxtSearch.BackColor = Color.FromArgb(24, 24, 24);
            TxtSearch.Font = new Font("Segoe UI", 14F);
            TxtSearch.ForeColor = Color.White;
            TxtSearch.Location = new Point(265, 4);
            TxtSearch.Name = "TxtSearch";
            TxtSearch.Size = new Size(526, 32);
            TxtSearch.TabIndex = 0;
            TxtSearch.TextAlign = HorizontalAlignment.Center;
            TxtSearch.TextChanged += TxtSearch_TextChanged;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top;
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Calibri", 16F, FontStyle.Bold);
            label2.ForeColor = Color.White;
            label2.Location = new Point(1294, 6);
            label2.Name = "label2";
            label2.Size = new Size(108, 27);
            label2.TabIndex = 2;
            label2.Text = "مسار الفيديو";
            // 
            // FrmItems
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1057, 450);
            Controls.Add(DGVData);
            Controls.Add(PnlSearch);
            Controls.Add(panel1);
            Name = "FrmItems";
            Text = "GT-Medical Items";
            WindowState = FormWindowState.Maximized;
            Controls.SetChildIndex(panel1, 0);
            Controls.SetChildIndex(PnlSearch, 0);
            Controls.SetChildIndex(DGVData, 0);
            ((System.ComponentModel.ISupportInitialize)DGVData).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            PnlSearch.ResumeLayout(false);
            PnlSearch.PerformLayout();
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
        private Panel PnlSearch;
        private TextBox TxtSearch;
        private Label label2;
    }
}