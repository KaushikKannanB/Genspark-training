namespace Inventory.Models.DTOs
{
    public class UpdateProductDescriptionDTO
    {
        public string ProductName { get; set; } = string.Empty;
        public string NewDescription { get; set; } = string.Empty;
    }
}