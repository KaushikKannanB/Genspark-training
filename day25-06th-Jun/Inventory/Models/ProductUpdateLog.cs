namespace Inventory.Models
{
    public class ProductUpdateLog
    {
        public string Id { get; set; } = string.Empty;
        public string ProductId { get; set; } = string.Empty;
        public string FieldUpdated { get; set; } = string.Empty;
        public string OldValue { get; set; } = string.Empty;
        public string NewValue { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTime UpdatedAt{ get; set; }
        public Product? Product { get; set; }
        public User?User { get; set; }
    }
}