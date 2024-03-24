using ClinicApp.Core.Models;
using FluentValidation;

namespace ClinicApp.WebApp.Services.Validations
{
    public class ServiceLogValidator : AbstractValidator<ServiceLog>
    {
        public ServiceLogValidator()
        {
            RuleFor(x => x.PeriodId)
                 .NotEmpty().WithMessage("Period ID is required.")
                 .GreaterThan(0).WithMessage("You must select a period");

            RuleFor(x => x.ContractorId)
                .NotEmpty().WithMessage("Contractor ID is required.")
                .GreaterThan(0).WithMessage("You must select a contractor.");

            RuleFor(x => x.ClientId)
                .NotEmpty().WithMessage("Client ID is required.")
                .GreaterThan(0).WithMessage("You must select a client");

            RuleFor(x => x.CreatedDate)
                .NotNull().WithMessage("Created date is required.");

            RuleFor(x => x.UnitDetails)
                .Must(x => x != null && x.Count > 0).WithMessage("At least one Unit Detail must be provided.");
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<ServiceLog>.CreateWithOptions((ServiceLog)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
