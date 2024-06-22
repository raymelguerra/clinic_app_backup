using ClinicApp.Core.Models;
using ClinicApp.WebApp.Interfaces;
using FluentValidation;

namespace ClinicApp.WebApp.Services.Validations;

public class InsuranceProcedureValidator : AbstractValidator<InsuranceProcedure>
{
    public InsuranceProcedureValidator()
    {
        RuleFor(p => p.Procedure)
                            .NotNull().WithMessage("Procedure cannot be null.");
        RuleFor(p => p.Rate)
            .NotNull().WithMessage("Rate cannot be null.")
            .GreaterThan(0);
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<InsuranceProcedure>.CreateWithOptions((InsuranceProcedure)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}