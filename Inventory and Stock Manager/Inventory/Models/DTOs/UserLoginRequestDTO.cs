using Inventory.Misc;
namespace Inventory.Models.DTOs
{
    public class UserLoginRequest
    {
        [EmailValidation]
        public string Email { get; set; } = string.Empty;
        [PasswordValidation]
        public string Password { get; set; } = string.Empty;
    }
}