using System.Text.Json.Serialization;

namespace Inventory.Models
{
    public class Product
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public float Price { get; set; }
        public string? CategoryId { get; set; }
        public string? InventoryId { get; set; }
        public DateTime AddedAt { get; set; }
        public string AddedBy { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        [JsonIgnore]
        public Category? Category { get; set; }
        [JsonIgnore]

        public Inventories? Inventory { get; set; }
        [JsonIgnore]
        
        public User? User { get; set; }
    }
}