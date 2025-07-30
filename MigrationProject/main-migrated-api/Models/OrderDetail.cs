using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MainMigration.Models
{
    public class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderDetailID { get; set; }

        public int OrderID { get; set; }
        public int ProductID { get; set; }

        public double? Price { get; set; }

        // Navigation properties
        [JsonIgnore]
        public Order? Order { get; set; }
        public Product? Product { get; set; }
    }
}
