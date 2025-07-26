using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MainMigration.Models
{
    public class Order
    {
        

        [Key]
        public int OrderID { get; set; }

        [Required]
        public string OrderName { get; set; } = string.Empty;

        public DateTime? OrderDate { get; set; }

        [Required]
        public string PaymentType { get; set; } = string.Empty;

        [Required]
        public string Status { get; set; } = string.Empty;

        [Required]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string CustomerPhone { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string CustomerEmail { get; set; } = string.Empty;

        [Required]
        public string CustomerAddress { get; set; } = string.Empty;

        // Navigation property
        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
