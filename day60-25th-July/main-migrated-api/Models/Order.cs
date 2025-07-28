using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MainMigration.Models
{
    public class Order
    {


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderID { get; set; }

        public DateTime? OrderDate { get; set; } = DateTime.Now;

        public double TotalAmount { get; set; }

        [Required]
        public string Status { get; set; } = string.Empty;

        // Navigation property
        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
