using System.Text.Json.Serialization;

namespace Inventory.Models
{
    public class StockLogging
    {
        public string Id { get; set; } = string.Empty;
        public string InventoryId { get; set; } = string.Empty;
        public int OldStock { get; set; }
        public int NewStock { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;

        [JsonIgnore]
        public Inventories? Inventory { get; set; }
        [JsonIgnore]
        public User? User { get; set; }

    }
}