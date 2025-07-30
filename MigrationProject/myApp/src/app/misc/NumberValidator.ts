import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export function numberValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;

    // Null, undefined, empty string, or whitespace-only
    if (value === null || value === undefined || String(value).trim() === '') {
      return { emptyError: 'Value is required' };
    }

    // Not a number
    const numericValue = Number(value);
    if (isNaN(numericValue)) {
      return { numberError: 'Value must be a number' };
    }

    // Negative check
    if (numericValue < 0) {
      return { negativeError: 'Number must not be negative' };
    }

    return null;
  };
}
