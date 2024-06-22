using ClinicApp.Core.Models;
using FluentValidation;

namespace ClinicApp.WebApp.Services.Validations
{
    public class InsuranceValidator : AbstractValidator<Insurance>
    {
        public InsuranceValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("The 'Name' field is required.")
                .NotNull().WithMessage("The 'Name' field cannot be null.")
                .Length(1, 100).WithMessage("The 'Name' field must be between 1 and 100 characters.");

            RuleFor(x => x.EntryDate)
            .NotNull().WithMessage("Entry date is required.");

            RuleFor(x => x.ExpirationDate)
              .NotNull().WithMessage("Expiration date is required.")
              .GreaterThan(x => x.EntryDate)
              .WithMessage("Expiration date must be greater than entry date.");

            RuleFor(x => x.InsuranceProcedures)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("The 'Procedures' list cannot be null.")
                .NotEmpty().WithMessage("The 'Procedures' list cannot be empty.")
                .Must(items => items != null && items.Any()).WithMessage("The 'Procedures' list must contain at least one element.")
                .ForEach(procedurelRule =>
                {
                    procedurelRule.ChildRules(procedure =>
                    {
                        procedure.RuleFor(p => p.Procedure)
                            .NotNull().WithMessage("Procedure cannot be null.");
                        procedure.RuleFor(p => p.Rate)
                            .NotNull().WithMessage("Rate cannot be null.")
                            .GreaterThan(0);

                    });
                });
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<Insurance>.CreateWithOptions((Insurance)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
