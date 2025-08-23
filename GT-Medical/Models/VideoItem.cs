using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GT_Medical.Models
{
    public class VideoItem
    {
        public required Guid ID { get; set; }
        public required string Barcode { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public required string LocalPath { get; set; }
        public required string RemoteUrl { get; set; }
        public string? Status { get; set; } = "Idle";
        [JsonIgnore]
        public int Progress { get; set; } = 0;
        public override bool Equals(object? obj)
        {
            if(obj != null && obj is VideoItem objItem)
            {
                return Barcode == objItem.Barcode
                    && Name == objItem.Name;
            }
            return base.Equals(obj);
        }
    }
}
