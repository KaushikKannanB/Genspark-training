namespace MainMigration.Models.DTOs
{
    public class AddProductDTO
    {
        public string ProductName { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public double Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int YearsUsed{ get; set; }
    }
}