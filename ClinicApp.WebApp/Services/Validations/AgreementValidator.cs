using ClinicApp.Core.Models;
using FluentValidation;

namespace ClinicApp.WebApp.Services.Validations;

public class AgreementValidator : AbstractValidator<Agreement>
{
    public AgreementValidator()
    {
        RuleFor(a => a.Company)
            .NotEmpty().WithMessage("Company is required.");

        RuleFor(a => a.Payroll)
            .NotEmpty().WithMessage("Payroll is required.");

        RuleFor(a => a.RateEmployees)
            .NotEmpty().WithMessage("RateEmployees is required.")
            .GreaterThan(0).WithMessage("RateEmployee must be greater than 0");
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<Agreement>.CreateWithOptions((Agreement)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}