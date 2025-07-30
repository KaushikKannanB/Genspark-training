import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export function textValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;

    // Check for null, empty, or whitespace-only
    if (!value || value.trim().length === 0) {
      return { emptyError: 'Text cannot be empty or just whitespace' };
    }

    // Check for length greater than 3 after trimming
    if (value.trim().length <= 3) {
      return { lengthError: 'Text must be longer than 3 characters' };
    }

    return null;
  };
}
