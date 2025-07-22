using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerAPI.DTOs
{
    public class EmailFileRequest
    {
        
        [Required]
        public string? Email { get; set; }

        [Required]
        public IFormFile? File { get; set; }
    }
}