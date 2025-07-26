using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MainMigration.Models
{
    public class Model
    {

        [Key]
        public int ModelId { get; set; }

        [Required]
        public string Model1 { get; set; } = string.Empty;

        public ICollection<Product> Products { get; set; }
    }
}
