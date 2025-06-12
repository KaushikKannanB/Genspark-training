using System;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Misc
{
    public class PositiveNumberValidation : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
                return false;

            try
            {
                double number = Convert.ToDouble(value);
                return number > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
