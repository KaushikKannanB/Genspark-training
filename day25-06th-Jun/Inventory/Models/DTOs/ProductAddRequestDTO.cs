using Inventory.Misc;
namespace Inventory.Models.DTOs
{
    public class ProductAddRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [PositiveNumberValidation]
        public float price { get; set; }
        public string categoryName { get; set; } = string.Empty;
        [PositiveNumberValidation]

        public int stock { get; set; }
        [PositiveNumberValidation]

        public int MinThreshold { get; set; }
    }
}