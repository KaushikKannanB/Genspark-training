using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MainMigration.Models
{
    public class Color
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ColorId { get; set; }

        [Required]
        public string ColorName { get; set; } = string.Empty;
        [JsonIgnore]
        public ICollection<Product>? Products { get; set; }
    }
}
