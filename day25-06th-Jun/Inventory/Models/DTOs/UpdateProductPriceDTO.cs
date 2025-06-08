namespace Inventory.Models.DTOs
{
    public class UpdateProductPriceDTO
    {
        public string ProductName { get; set; } = string.Empty;
        public float newprice{ get; set; }
    }
}