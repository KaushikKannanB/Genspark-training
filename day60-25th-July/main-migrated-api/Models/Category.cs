using System.ComponentModel.DataAnnotations;

namespace MainMigration.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        // Navigation property
        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
