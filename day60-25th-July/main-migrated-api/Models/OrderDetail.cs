using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MainMigration.Models
{
    public class OrderDetail
    {
        [Key, Column(Order = 0)]
        public int OrderID { get; set; }

        [Key, Column(Order = 1)]
        public int ProductID { get; set; }

        public double? Price { get; set; }

        public int? Quantity { get; set; }

        // Navigation properties
        public Order? Order { get; set; }
        public Product? Product { get; set; }
    }
}
