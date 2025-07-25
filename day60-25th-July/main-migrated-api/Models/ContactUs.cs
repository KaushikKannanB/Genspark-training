using System.ComponentModel.DataAnnotations;

namespace MainMigration.Models
{
    public class ContactUs
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;
    }
}
