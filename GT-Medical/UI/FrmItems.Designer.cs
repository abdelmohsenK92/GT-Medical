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
            ((System.ComponentModel.ISupportInitialize)DGVData).BeginInit();
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
            DGVData.Size = new Size(800, 415);
            DGVData.TabIndex = 0;
            DGVData.CellBeginEdit += DGVData_CellBeginEdit;
            DGVData.CellValueChanged += DGVData_CellValueChanged;
            // 
            // FrmItems
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(DGVData);
            Name = "FrmItems";
            Text = "GT-Medical Items";
            WindowState = FormWindowState.Maximized;
            Controls.SetChildIndex(DGVData, 0);
            ((System.ComponentModel.ISupportInitialize)DGVData).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView DGVData;
    }
}