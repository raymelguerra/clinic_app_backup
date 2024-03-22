using ClinicApp.Core.Models;
using FluentValidation;

namespace ClinicApp.WebApp.Services.Validations;

public class PatientAccountValidator : AbstractValidator<PatientAccount>
{
    public PatientAccountValidator()
    {
        RuleFor(x => x.LicenseNumber)
         .NotEmpty().When(x => string.IsNullOrEmpty(x.Auxiliar))
             .WithMessage("License number is required when Auxiliar is empty.")
         .MaximumLength(50).WithMessage("License number must not exceed 50 characters.");

        RuleFor(x => x.Auxiliar)
            .NotEmpty().When(x => string.IsNullOrEmpty(x.LicenseNumber))
                .WithMessage("Auxiliar is required when License number is empty.")
            .MaximumLength(100).WithMessage("Auxiliar must not exceed 100 characters.");

        RuleFor(x => x.CreateDate)
            .NotNull().WithMessage("Create date is required.");

        RuleFor(x => x.ExpireDate)
          .NotNull().WithMessage("Expire date is required.")
          .GreaterThan(x => x.CreateDate).WithMessage("Expire date must be greater than create date.");

        RuleFor(x => x.ClientId)
            .NotEmpty().WithMessage("Client ID is required.")
            .GreaterThan(0).WithMessage("Client ID must be greater than 0.");
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<PatientAccount>.CreateWithOptions((PatientAccount)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };

    private IEnumerable<string> ValidateFieldValue(PatientAccount arg)
    {
        var result = Validate(arg);
        if (result.IsValid)
            return new string[0];
        return result.Errors.Select(e => e.ErrorMessage);
    }

    public Func<PatientAccount, IEnumerable<string>> Validation => ValidateFieldValue;
}
