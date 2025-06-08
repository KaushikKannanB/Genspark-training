using System.Text.Json.Serialization;

namespace Inventory.Models
{
    public class Inventories
    {
        public string Id { get; set; } = string.Empty;
        public int Stock { get; set; }
        public int MinThreshold { get; set; }
        
        [JsonIgnore]
        public Product? Product { get; set; }
    }
}