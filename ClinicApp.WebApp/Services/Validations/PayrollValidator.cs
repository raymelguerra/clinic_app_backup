using ClinicApp.Core.Models;
using FluentValidation;

namespace ClinicApp.WebApp.Services.Validations;

public class PayrollValidator : AbstractValidator<Payroll>
{
    public PayrollValidator()
    {
        RuleFor(p => p.ContractorType)
            .NotNull().WithMessage("Contractor type cannot be null.");

        RuleFor(p => p.InsuranceProcedure)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("InsuranceProcedure cannot be null.");

        RuleFor(p => p.InsuranceProcedure.Insurance)
            .NotNull().WithMessage("Insurance cannot be null.")
            .When(p => p.InsuranceProcedure != null);

        RuleFor(p => p.InsuranceProcedure.Procedure)
            .NotNull().WithMessage("Procedure cannot be null.")
            .When(p => p.InsuranceProcedure != null);
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<Payroll>.CreateWithOptions((Payroll)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}