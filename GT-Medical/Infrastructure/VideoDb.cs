using GT_Medical.Helper.Extensions;
using GT_Medical.Models;
using Newtonsoft.Json;
using SharpCompress.Archives;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel; // ISynchronizeInvoke
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GT_Medical.Infrastructure
{
    /// <summary>
    /// Video database backed by a JSON file (db.dll) and a physical "videos" folder.
    /// - First run: scans the folder, generates barcodes starting at 1001, saves JSON.
    /// - Next runs: loads JSON, then merges any new files in the folder.
    /// - Exposes a bindable collection (VideoItemsBindingCollection) for UI binding.
    /// - All modifications to the binding collection are marshaled to the UI thread via RunOnUi.
    /// </summary>
    public class VideoDb : CrossThreadInvoker
    {
        private readonly string _dbPath;
        private readonly string _videosDir;

        // Simple synchronization for snapshots (Save/GetByBarcode). UI list changes are marshaled via RunOnUi.
        private readonly object _sync = new object();

        // Bindable list for UI (assumed: derived from BindingList<VideoItem>)
        private readonly VideoItemsBindingCollection _items = new VideoItemsBindingCollection();
        public VideoItemsBindingCollection Items => _items;

    
        public VideoDb(ISynchronizeInvoke uiInvoker) : base(uiInvoker)
        {
            string appBase = AppDomain.CurrentDomain.BaseDirectory;
            _dbPath = Path.Combine(appBase, "db.dll");
            _videosDir = Path.Combine(appBase, "videos");
            Directory.CreateDirectory(_videosDir);
            //ExtractVideos();
        }

        private void ExtractVideos()
        {
            string videosFolder = AppDomain.CurrentDomain.BaseDirectory + "\\Videos";
            if (Directory.Exists(videosFolder))
            {
                var archiveFile = videosFolder + ".rar";
                if (File.Exists(archiveFile))
                {
                    using (var archive = ArchiveFactory.Open(archiveFile))
                    {
                        foreach (var entry in archive.Entries)
                        {
                            if (!entry.IsDirectory)
                            {
                                Console.WriteLine($"Extracting: {entry.Key}");
                                entry.WriteToDirectory(AppDomain.CurrentDomain.BaseDirectory, new ExtractionOptions()
                                {
                                    ExtractFullPath = true,   // keep folder structure
                                    Overwrite = true          // overwrite if file exists
                                });
                            }
                        }
                    }
                }
            }
        }



        /// <summary>
        /// Initialize: if JSON does not exist, scan the "videos" folder and seed items; otherwise load.
        /// Then merge any new physical files found under the folder.
        /// </summary>
        public async Task InitializeAsync(string[] allowedExtensions = null)
        {
            allowedExtensions ??= new[] { ".mp4", ".mov", ".mkv", ".avi", ".wmv" };
            
            if (!File.Exists(_videosDir))
                Directory.CreateDirectory(_videosDir);
            if (!File.Exists(_dbPath))
            {
                // Fresh scan and seed
                var files = Directory
                    .EnumerateFiles(_videosDir, "*.*", SearchOption.AllDirectories)
                    .Where(f => allowedExtensions.Contains(Path.GetExtension(f), StringComparer.OrdinalIgnoreCase))
                    .OrderBy(f => f, StringComparer.OrdinalIgnoreCase)
                    .ToList();

                    long next = 1001;
                    var seed = new List<VideoItem>();
                    foreach (var f in files)
                    {
                        seed.Add(new VideoItem
                        {
                            ID = Guid.NewGuid(),
                            Barcode = next.ToString(),
                            Name = Path.GetFileNameWithoutExtension(f),
                            LocalPath = f,
                            RemoteUrl = string.Empty,
                            Status = File.Exists(f) ? "Idle" : "Missing"
                        });
                        next++;
                    }
                    RunOnUi(() =>
                    {
                        _items.RaiseListChangedEvents = false;
                        _items.Clear();
                        foreach (var v in seed) _items.Add(v);
                        _items.RaiseListChangedEvents = true;
                        _items.ResetBindings();
                    });
                
                    Directory.CreateDirectory(_videosDir);
                    

                await SaveAsync();
            }
            else
            {
                await LoadAsync();
            }

            await MergeNewFilesAsync(allowedExtensions);
        }

        /// <summary>
        /// Load from db.dll into the binding list (UI thread).
        /// </summary>
        public async Task LoadAsync()
        {
            List<VideoItem> loaded;
            if (!File.Exists(_dbPath))
            {
                loaded = new List<VideoItem>();
            }
            else
            {
                string json = await Task.Run(() => File.ReadAllText(_dbPath));
                loaded = JsonConvert.DeserializeObject<List<VideoItem>>(json) ?? new List<VideoItem>();
            }

            // Normalize fields
            foreach (var it in loaded)
            {
                if (string.IsNullOrWhiteSpace(it.Status))
                    it.Status = File.Exists(it.LocalPath) ? "Idle" : "Missing";
                if (it.RemoteUrl == null)
                    it.RemoteUrl = string.Empty;
            }

            RunOnUi(() =>
            {
                _items.RaiseListChangedEvents = false;
                _items.Clear();
                foreach (var v in loaded) _items.Add(v);
                _items.RaiseListChangedEvents = true;
                _items.ResetBindings();
            });
        }

        /// <summary>
        /// Save binding list snapshot to db.dll using a temp file + replace (atomic where possible).
        /// </summary>
        public async Task SaveAsync()
        {
            List<VideoItem> snapshot;
            lock (_sync)
            {
                // Snapshot to avoid enumerating while UI thread modifies the list
                snapshot = _items.ToList();
            }

            var json = JsonConvert.SerializeObject(snapshot, Formatting.Indented);
            var tmp = _dbPath + ".tmp";

            await Task.Run(() => File.WriteAllText(tmp, json));
            
            if (File.Exists(_dbPath)) 
                File.Replace(tmp, _dbPath, null);
            else 
                File.Move(tmp, _dbPath);
        }

        /// <summary>
        /// Returns a read-only copy of an item by barcode (safe outside UI thread).
        /// </summary>
        public VideoItem GetByBarcode(string barcode)
        {
            lock (_sync)
            {
                var it = _items.FirstOrDefault(x => string.Equals(x.Barcode, barcode, StringComparison.OrdinalIgnoreCase));
                return it == null ? null : CloneForRead(it);
            }
        }

        /// <summary>
        /// Add new or update existing item. Marshaled to the UI thread.
        /// </summary>
        public async Task<VideoItem> AddOrUpdateAsync(VideoItem item,
            bool autoSave = true)
        {
            if (item == null) 
                throw new ArgumentNullException(nameof(item));
            VideoItem result = null;

            RunOnUi(() =>
            {
                if (string.IsNullOrWhiteSpace(item.Barcode))
                    item.Barcode = NextBarcodeUiThread();

                var existing = _items.FirstOrDefault(x => x.Barcode == item.Barcode);
                if (existing == null)
                {
                    item.Status = File.Exists(item.LocalPath) ? "Idle" : "Missing";
                    _items.Add(item);
                    result = item;
                }
                else
                {
                    existing.Name = item.Name;
                    existing.Description = item.Description;
                    existing.LocalPath = item.LocalPath;
                    existing.RemoteUrl = item.RemoteUrl ?? string.Empty;
                    existing.Status = File.Exists(item.LocalPath) ? "Idle" : "Missing";
                    _items.ResetItem(_items.IndexOf(existing));
                    result = existing;
                }
            });

            if (autoSave) 
                await SaveAsync();
            return CloneForRead(result);
        }

        /// <summary>
        /// Update item by ID. Marshaled to the UI thread.
        /// </summary>
        public async Task<bool> UpdateAsync(VideoItem item, bool autoSave = true)
        {
            bool updated = false;

            RunOnUi(() =>
            {
                var itemToUpdate = _items.FirstOrDefault(x => x.ID.Equals(item.ID));
                if (itemToUpdate != null)
                {
                    itemToUpdate.Barcode = item.Barcode;
                    itemToUpdate.Name = item.Name;
                    itemToUpdate.Description = item.Description;
                    itemToUpdate.LocalPath = item.LocalPath;
                    itemToUpdate.RemoteUrl = item.RemoteUrl ?? string.Empty;
                    itemToUpdate.Status = File.Exists(item.LocalPath) ? "Idle" : "Missing";
                    _items.ResetItem(_items.IndexOf(itemToUpdate));
                    updated = true;
                }
            });

            if (updated && autoSave) 
                await SaveAsync();
            return updated;
        }

        /// <summary>
        /// Update Status by barcode. Marshaled to the UI thread.
        /// </summary>
        public async Task<bool> UpdateStatusAsync(string barcode, string status, bool autoSave = true)
        {
            bool updated = false;

            RunOnUi(() =>
            {
                var it = _items.FirstOrDefault(x => x.Barcode == barcode);
                if (it != null)
                {
                    it.Status = status;
                    _items.ResetItem(_items.IndexOf(it));
                    updated = true;
                }
            });

            if (updated && autoSave) await SaveAsync();
            return updated;
        }
        /// <summary>
        /// Update LocalPath by barcode (e.g., after download). Marshaled to the UI thread.
        /// </summary>
        public async Task<bool> UpdateLocalPathAsync(string barcode, string newLocalPath, bool autoSave = true)
        {
            bool updated = false;

            RunOnUi(() =>
            {
                var it = _items.FirstOrDefault(x => x.Barcode == barcode);
                if (it != null)
                {
                    it.LocalPath = newLocalPath;
                    it.Status = File.Exists(newLocalPath) ? "Idle" : "Missing";
                    _items.ResetItem(_items.IndexOf(it));
                    updated = true;
                }
            });

            if (updated && autoSave) await SaveAsync();
            return updated;
        }

        /// <summary>
        /// Rescan the "videos" folder and add any new files with new barcodes. Marshaled to the UI thread.
        /// Also refreshes Status for existing rows based on file existence.
        /// </summary>
        public async Task<int> MergeNewFilesAsync(string[] allowedExtensions = null)
        {
            allowedExtensions ??= new[] { ".mp4", ".mov", ".mkv", ".avi", ".wmv" };
            var files = Directory
                .EnumerateFiles(_videosDir, "*.*", SearchOption.AllDirectories)
                .Where(f => allowedExtensions.Contains(Path.GetExtension(f), StringComparer.OrdinalIgnoreCase))
                .ToList();

            int added = 0;

            RunOnUi(() =>
            {
                var knownPaths = new HashSet<string>(_items.Select(x => x.LocalPath), StringComparer.OrdinalIgnoreCase);

                foreach (var f in files)
                {
                    if (knownPaths.Contains(f))
                        continue;

                    var item = new VideoItem
                    {
                        ID = new Guid(),
                        Barcode = NextBarcodeUiThread(),
                        Name = Path.GetFileNameWithoutExtension(f),
                        LocalPath = f,
                        RemoteUrl = string.Empty,
                        Status = "Idle"
                    };
                    _items.Add(item);
                    added++;
                }

                // Refresh status for existing rows
                for (int i = 0; i < _items.Count; i++)
                {
                    var it = _items[i];
                    if (!string.IsNullOrWhiteSpace(it.LocalPath))
                    {
                        var newStatus = File.Exists(it.LocalPath) ? "Idle" : "Missing";
                        if (!string.Equals(newStatus, it.Status, StringComparison.Ordinal))
                        {
                            it.Status = newStatus;
                            _items.ResetItem(i);
                        }
                    }
                }
            });

            if (added > 0)
                await SaveAsync();
            return added;
        }

        /// <summary>
        /// Generate next barcode by inspecting current items. Must be called on UI thread.
        /// Starts at 1001 if list is empty.
        /// </summary>
        private string NextBarcodeUiThread()
        {
            long max = 1000;
            foreach (var b in _items.Select(i => i.Barcode))
            {
                if (long.TryParse(b, out var n) && n > max) max = n;
            }
            return (max + 1).ToString();
        }

        /// <summary>
        /// Read-only clone for safe returns outside the UI thread.
        /// </summary>
        private static VideoItem CloneForRead(VideoItem originalItem)
        {
            if (originalItem == null) 
                return null;
            return new VideoItem
            {
                ID = originalItem.ID,
                Barcode = originalItem.Barcode,
                Name = originalItem.Name,
                Description = originalItem.Description,
                LocalPath = originalItem.LocalPath,
                RemoteUrl = originalItem.RemoteUrl,
                Status = originalItem.Status
            };
        }

       
    }
}
