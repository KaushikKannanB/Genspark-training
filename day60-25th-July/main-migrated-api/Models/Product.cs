using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MainMigration.Models
{
    public class Product
    {

        [Key]
        public int ProductId { get; set; }

        [Required]
        public string ProductName { get; set; } = string.Empty;

        public string? Image { get; set; }

        public double? Price { get; set; }

        public int? UserId { get; set; }
        public int? CategoryId { get; set; }
        public int? ColorId { get; set; }
        public int? ModelId { get; set; }
        public int? StorageId { get; set; }

        public DateTime? SellStartDate { get; set; }
        public DateTime? SellEndDate { get; set; }

        public int? IsNew { get; set; }

        // Navigation properties
        public Category? Category { get; set; }
        public Color? Color { get; set; }
        public Model? Model { get; set; }
        public User? User { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
