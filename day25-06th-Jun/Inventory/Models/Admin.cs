using System.Text.Json.Serialization;

namespace Inventory.Models
{
    public class Admin
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        [JsonIgnore]
        public User? User { get; set; }
    }
}