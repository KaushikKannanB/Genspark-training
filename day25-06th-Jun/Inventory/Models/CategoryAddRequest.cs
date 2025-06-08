using System.Text.Json.Serialization;

namespace Inventory.Models
{
    public class CategoryAddRequest
    {
        public string Id { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string RequestedBy { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        [JsonIgnore]
        public User? User { get; set; }
    }
}