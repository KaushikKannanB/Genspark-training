using Inventory.Misc;
namespace Inventory.Models.DTOs
{
    public class StockUpdateDTO
    {
        public string ProductName { get; set; } = string.Empty;
        public string AddOrReduce { get; set; } = string.Empty;
        [PositiveNumberValidation]
        public int AddOrReduceBy { get; set; }
    }
}