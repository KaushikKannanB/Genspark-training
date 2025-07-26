using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MainMigration.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public ICollection<News> News { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
