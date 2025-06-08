namespace Inventory.Models
{
    public class User
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public Admin? Admin { get; set; }
        public Manager? Manager { get; set; }
        public ICollection<Product>? Products { get; set; }
        public ICollection<Category>? Categories { get; set; }
        public ICollection<CategoryAddRequest>? CategoryAdds { get; set; }
        public ICollection<ProductUpdateLog>? ProductLogs { get; set; }
        public ICollection<StockLogging>? StockLogs { get; set; }

        
        
        
    }
}