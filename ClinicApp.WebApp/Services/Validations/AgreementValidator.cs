using ClinicApp.Core.Models;
using FluentValidation;

namespace ClinicApp.WebApp.Services.Validations;

public class AgreementValidator : AbstractValidator<Agreement>
{
    public AgreementValidator()
    {
        RuleFor(a => a.Company)
            .NotEmpty().WithMessage("Company es requerido.");

        RuleFor(a => a.Payroll)
            .NotEmpty().WithMessage("Payroll es requerido.");

        RuleFor(a => a.RateEmployees)
            .NotEmpty().WithMessage("RateEmployees es requerido.")
            .GreaterThan(0).WithMessage("RateEmployees debe ser mayor que cero.");
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<Agreement>.CreateWithOptions((Agreement)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}