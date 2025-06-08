namespace Inventory.Models.DTOs
{
    public class ProductAddRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public float price { get; set; }
        public string categoryName { get; set; } = string.Empty;
        public int stock { get; set; }
        public int MinThreshold{ get; set; }
    }
}