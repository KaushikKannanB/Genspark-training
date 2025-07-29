using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MainMigration.Models
{
    public class Order
    {


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderID { get; set; }

        public DateTime? OrderDate { get; set; } = DateTime.Now;

        public double TotalAmount { get; set; }

        public int UserId{ get; set; }
        [Required]
        public string Status { get; set; } = string.Empty;

        // Navigation property
        [JsonIgnore]
        public User? User { get; set; }
        [JsonIgnore]
        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
