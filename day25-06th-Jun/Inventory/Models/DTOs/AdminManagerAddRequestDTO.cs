using Inventory.Misc;
namespace Inventory.Models.DTOs
{
    public class AdminManagerAddRequestDTO
    {
        public string Name { get; set; } = string.Empty;
        [EmailValidation]
        public string Email { get; set; } = string.Empty;
        [PasswordValidation]
        public string Password { get; set; } = string.Empty;

    }
}