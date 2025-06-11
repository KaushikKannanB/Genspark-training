using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Inventory.Misc
{
    public class EmailValidation : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            string email = value as string ?? "";

            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Simple but effective email pattern
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            return Regex.IsMatch(email, emailPattern, RegexOptions.IgnoreCase);
        }
    }
}
