namespace Inventory.Models.DTOs
{
    public class StockUpdateDTO
    {
        public string ProductName { get; set; } = string.Empty;
        public string AddOrReduce { get; set; } = string.Empty;
        public int AddOrReduceBy{ get; set; }
    }
}