
using System.ComponentModel.DataAnnotations;

namespace MainMigration.Models.DTOs
{
    public class UserSignUpRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        [Phone]
        public string CustomerPhone { get; set; } = string.Empty;

        [EmailAddress]
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerAddress { get; set; } = string.Empty;
    }
}