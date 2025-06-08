using System.Text.Json.Serialization;

namespace Inventory.Models
{
    public class Category
    {
        public string Id { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;

        [JsonIgnore]
        public User? User { get; set; }
        public ICollection<Product>? Products{ get; set; }
    }
}