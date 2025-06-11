using Inventory.Misc;
namespace Inventory.Models.DTOs
{
    public class UpdateProductPriceDTO
    {
        public string ProductName { get; set; } = string.Empty;
        [PositiveNumberValidation]
        public float newprice { get; set; }
    }
}