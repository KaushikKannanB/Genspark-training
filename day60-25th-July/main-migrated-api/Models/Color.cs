using System.ComponentModel.DataAnnotations;

namespace MainMigration.Models
{
    public class Color
    {
        [Key]
        public int ColorId { get; set; }

        [Required]
        public string ColorName { get; set; } = string.Empty;

        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
