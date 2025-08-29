using GT_Medical.Abstractions;
using GT_Medical.Infrastructure;
using GT_Medical.Models;
using GT_Medical.Services;
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
    public partial class FrmItems : BaseForm, ITransientService
    {
        private VideoDb _db;
        private VideoItem _selectedItem;

        public FrmItems() : base()
        {
            if (DesignMode) 
                return;

            InitializeComponent();
            StyleGrid(DGVData);
            AddGridColumns(DGVData);
            StyleGridButtons(DGVData);
            DGVData.CellClick += DGVData_CellClick;
        }

        private void DGVData_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            var index = e.ColumnIndex;
            var name = DGVData.Columns[e.ColumnIndex].Name;
        }

        public FrmItems(VideoDb db) : this()
        {
            _db = db;
            DGVData.DataSource = _db.Items;
            Load += FrmItems_Load;
            //FitColumns(DGVData);
        }

        private void FrmItems_Load(object? sender, EventArgs e)
        {
            TxtPath.Text = AppSettings.Current.LocalVideosUrl;
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
                Name = nameof(VideoItem.ID),
                DataPropertyName = nameof(VideoItem.ID),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                Visible = false,
                DisplayIndex = 0
            });

            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "الباركود",
                Name = nameof(VideoItem.Barcode),
                DataPropertyName = nameof(VideoItem.Barcode),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 75,
                DisplayIndex = 1
            });

            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "اسم الصنف",
                Name = nameof(VideoItem.Name),
                DataPropertyName = nameof(VideoItem.Name),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 75,
                DisplayIndex = 2
            });

            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "الوصف",
                DataPropertyName = nameof(VideoItem.Description),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 150,
                DisplayIndex = 3

            });

            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "رابط الفيديو",
                Name = nameof(VideoItem.LocalPath),
                DataPropertyName = nameof(VideoItem.LocalPath),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true,
                Visible = false,
                DisplayIndex = 4
            });

            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "رابط التحميل",
                DataPropertyName = nameof(VideoItem.RemoteUrl),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true,
                Visible = false,
                DisplayIndex = 5
            });

            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Status",
                Name = nameof(VideoItem.Status),
                DataPropertyName = nameof(VideoItem.Status),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true,
                Visible = false,
                DisplayIndex = 6
            });

            g.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Progress %",
                Name = nameof(VideoItem.Progress),
                DataPropertyName = nameof(VideoItem.Progress),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true,
                Visible = false,
                DisplayIndex = 7
            });

            var btnCol = new DataGridViewButtonColumn
            {
                HeaderText = "",
                Text = "تحميل ⬇",
                Name = "Load",
                UseColumnTextForButtonValue = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 40,
                DisplayIndex = 8
            };

            g.Columns.Add(btnCol);

            var btnPlayCol = new DataGridViewButtonColumn
            {
                HeaderText = "",
                Text = "تشغيل ▶",
                Name = "Play",
                UseColumnTextForButtonValue = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 40,
                DisplayIndex = 9
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
            if (_selectedItem != null)
                await _db.UpdateAsync(_selectedItem, true);
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

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            DlgFolderBrowser.InitialDirectory = AppSettings.Current.LocalVideosUrl;
            var dlgResult = DlgFolderBrowser.ShowDialog();
            if (dlgResult == DialogResult.OK)
            {
                TxtPath.Text = DlgFolderBrowser.SelectedPath;
                AppSettings.Current = new AppSettings { LocalVideosUrl = DlgFolderBrowser.SelectedPath };
            }
        }

        private async void DGVData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > 1)
                return;

            if(e.ColumnIndex == 0)
            {
                using var dlg = new OpenFileDialog();
                dlg.Title = "اختر ملف الفيديو";
                dlg.Filter = "Video Files|*.mp4;*.avi;*.mkv;*.mov;*.wmv;*.flv;*.webm|All Files|*.*";
                var result = dlg.ShowDialog();
                if (result == DialogResult.OK)
                {
                    try
                    {
                        var item = (VideoItem)DGVData.Rows[e.RowIndex].DataBoundItem;
                        if (item != null)
                        {
                            item.LocalPath = dlg.FileName;
                            DGVData.Refresh();
                            await _db.UpdateAsync(item, true);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            else if(e.ColumnIndex == 1)
            {
                try
                {
                    var item = (VideoItem)DGVData.Rows[e.RowIndex].DataBoundItem;
                    if (item != null && !string.IsNullOrWhiteSpace(item.LocalPath))
                    {
                        var baseForm =  new BaseForm()
                        {
                         WindowState = FormWindowState.Maximized   
                        };
                        var videplayer = new VlcPlayerControl()
                        {
                            Dock = DockStyle.Fill,
                        };
                        baseForm.Controls.Add(videplayer);
                        baseForm.SetTitle(item.Name);
                        VideoPlayer player = new VideoPlayer(baseForm,videplayer,baseForm.ShowTip);
                        baseForm.Shown += async (s, ev) =>
                        {
                            videplayer.BringToFront();
                            await player.InitializeAsync();
                            player.PlayLocal(item.LocalPath);
                        };
                        baseForm.ShowDialog(this);
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}
