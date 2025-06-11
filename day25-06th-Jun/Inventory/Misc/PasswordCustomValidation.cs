using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Inventory.Misc
{
    public class PasswordValidation : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            string password = value as string ?? "";

            if (string.IsNullOrWhiteSpace(password))
                return false;

            if (password.Length < 8)
                return false;

            bool hasUpper = Regex.IsMatch(password, "[A-Z]");
            bool hasLower = Regex.IsMatch(password, "[a-z]");
            bool hasDigit = Regex.IsMatch(password, "[0-9]");
            bool hasSpecial = Regex.IsMatch(password, @"[\W_]");

            return hasUpper && hasLower && hasDigit && hasSpecial;
        }
    }
}
