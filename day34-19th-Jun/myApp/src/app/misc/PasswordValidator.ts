import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";


export function passwordStrengthValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;

    if (!value) return null; // Don't validate empty values here (handled by required validator)

    const hasMinLength = value.length >= 8;
    const hasUpperCase = /[A-Z]/.test(value);
    const hasNumber = /\d/.test(value);
    const hasSpecialChar = /[!@#$%^&*(),.?":{}|<>]/.test(value);

    const isValid = hasMinLength && hasUpperCase && hasNumber && hasSpecialChar;

    return isValid ? null : {passwordError:"Invalid password"};
  };
}