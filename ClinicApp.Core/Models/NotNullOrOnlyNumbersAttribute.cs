using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ClinicApp.Core.Models;

public class NotNullOrOnlyNumbersAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
    {
        if (value is null)
            return new ValidationResult(ErrorMessage);

        string stringValue = value.ToString();

        if (stringValue?.Length > 100)
            return new ValidationResult($"{ErrorMessage}, can't be longer than 100 characters");

        if (stringValue.All(char.IsDigit))
            return new ValidationResult($"{ErrorMessage}, can't contain only numbers");

        if (!Regex.IsMatch(stringValue, @"^[a-zA-Z0-9_\-*\.]+$"))
            return new ValidationResult($"{ErrorMessage}, can't contain special characters");

        return ValidationResult.Success;
    }
}

