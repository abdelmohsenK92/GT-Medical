using GT_Medical.Infrastructure;
using GT_Medical.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GT_Medical.UI
{
    public partial class FrmItems : BaseForm
    {
        private VideoDb _db;
        private VideoItem _selectedItem;
        public FrmItems() : base()
        {
            if (DesignMode) return;
            InitializeComponent();
            StyleGrid(DGVData);
            AddGridColumns(DGVData);
            StyleGridButtons(DGVData);
        }
        public FrmItems(VideoDb db) : this()
        {
            _db = db;
            DGVData.DataSource = _db.Items;
            //FitColumns(DGVData);
        }

        private void StyleGrid(DataGridView g)
        {
            g.DefaultCellStyle.BackColor = Color.FromArgb(28, 40, 52);
            g.DefaultCellStyle.ForeColor = Color.White;
            g.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 170, 140);
            g.DefaultCellStyle.SelectionForeColor = Color.White;
            g.GridColor = Color.FromArgb(50, 70, 90);
        }
        // call this once (e.g., in Form_Load)
        private void StyleGridButtons(DataGridView grid)
        {
            grid.CellPainting -= Grid_CellPaintingButtons;
            grid.CellPainting += Grid_CellPaintingButtons;
            grid.CellMouseEnter -= Grid_CellMouseEnterButtons;
            grid.CellMouseEnter += Grid_CellMouseEnterButtons;
            grid.CellMouseLeave -= Grid_CellMouseLeaveButtons;
            grid.CellMouseLeave += Grid_CellMouseLeaveButtons;
            grid.EnableHeadersVisualStyles = false;
        }

        private int _hoverRow = -1, _hoverCol = -1;

        private void Grid_CellMouseEnterButtons(object sender, DataGridViewCellEventArgs e)
        {
            _hoverRow = e.RowIndex; _hoverCol = e.ColumnIndex;
        }

        private void Grid_CellMouseLeaveButtons(object sender, DataGridViewCellEventArgs e)
        {
            _hoverRow = e.RowIndex; _hoverCol = e.ColumnIndex;
        }

        private void Grid_CellPaintingButtons(object sender, DataGridViewCellPaintingEventArgs e)
        {
            var grid = (DataGridView)sender;
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            if (!(grid.Columns[e.ColumnIndex] is DataGridViewButtonColumn)) 
                return;

            e.Handled = true;
            e.PaintBackground(e.CellBounds, true);

            // colors
            var isHover = (e.RowIndex == _hoverRow && e.ColumnIndex == _hoverCol);
            var bg = isHover ? Color.FromArgb(0, 150, 128) : Color.DarkSlateGray;
            var fore = Color.White;

            using (var b = new SolidBrush(bg))
                e.Graphics.FillRectangle(b, e.CellBounds);

            // border
            using (var pen = new Pen(Color.FromArgb(0, 120, 100)))
                e.Graphics.DrawRectangle(pen, e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width - 1, e.CellBounds.Height - 1);

            // text
            var text = (e.FormattedValue ?? "").ToString();
            using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            using (var f = new Font("Segoe UI Semibold", 9f))
            using (var br = new SolidBrush(fore))
                e.Graphics.DrawString(text, f, br, e.CellBounds, sf);
        }

        private void AddGridColumns(DataGridView g)
        {
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "ID",
                DataPropertyName = nameof(VideoItem.ID),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                Visible = false
            });

            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "الباركود",
                DataPropertyName = nameof(VideoItem.Barcode),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 75,
            });

            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "اسم الصنف",
                DataPropertyName = nameof(VideoItem.Name),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 75,
            });

            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "الوصف",
                DataPropertyName = nameof(VideoItem.Description),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 150,
            });

            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "رابط الفيديو",
                DataPropertyName = nameof(VideoItem.LocalPath),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true,
                Visible = false
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "رابط التحميل",
                DataPropertyName = nameof(VideoItem.RemoteUrl),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true,
                Visible = false
            });
            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Status",
                DataPropertyName = nameof(VideoItem.Status),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true,
                Visible = false
            });

            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Progress %",
                DataPropertyName = nameof(VideoItem.Progress),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true,
                Visible = false               
            });


            var btnCol = new DataGridViewButtonColumn
            {
                HeaderText = "",
                Text = "تحميل ⬇",
                UseColumnTextForButtonValue = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 40,
                ReadOnly = true,                
            };
            g.Columns.Add(btnCol);

            var btnPlayCol = new DataGridViewButtonColumn
            {
                HeaderText = "",
                Text = "تشغيل ▶",
                UseColumnTextForButtonValue = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 40,              
                               
            };
            g.Columns.Add(btnPlayCol);
        }
        private void FitColumns(DataGridView grid, int maxWidth = 380)
        {
            // 1) base autosize by content
            foreach (DataGridViewColumn c in grid.Columns)
                c.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None; // we'll set Widths explicitly

            // 2) compute preferred widths
            grid.SuspendLayout();
            foreach (DataGridViewColumn c in grid.Columns)
            {
                var w = c.GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCells, true);
                // cap
                var capped = Math.Min(w + 8, maxWidth);
                c.Width = Math.Max(50, capped);
            }
            grid.ResumeLayout();
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            // (اختياري) عمود طويل نخليه Fill ضمن حدود
            var localPathCol = grid.Columns.Cast<DataGridViewColumn>()
                .FirstOrDefault(col => col.HeaderText.Contains("Local", StringComparison.OrdinalIgnoreCase));
            if (localPathCol != null)
            {
                localPathCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                localPathCol.MinimumWidth = 120;
                localPathCol.FillWeight = 200; // ياخد المساحة الفاضلة
            }
        }

        private async void DGVData_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if(_selectedItem != null)
                await _db.UpdateAsync(_selectedItem,true);
        }

        private void DGVData_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                _selectedItem = (VideoItem)DGVData.CurrentRow.DataBoundItem;
            }
            catch (Exception ex)
            {
            }
        }
    }
}
