using ClinicApp.Core.Models;
using FluentValidation;

namespace ClinicApp.WebApp.Services.Validations
{
    public class PeriodValidator : AbstractValidator<Period>
    {
        public PeriodValidator()
        {
            RuleFor(x => x.PayPeriod)
                .NotEmpty().WithMessage("The 'Pay Period' field is required.")
                .NotNull().WithMessage("The 'Pay Period' field cannot be null.")
                .Length(1, 100).WithMessage("The 'Pay Period' field must be between 1 and 100 characters.");

            RuleFor(x => x.StartDate)
            .NotNull().WithMessage("Start date is required.");

            RuleFor(x => x.EndDate)
              .NotNull().WithMessage("End date is required.")
              .GreaterThan(x => x.StartDate)
              .WithMessage("End date must be greater than start date.");

            RuleFor(x => x.DocumentDeliveryDate)
              .NotNull().WithMessage("Document delivery date is required.")
              .GreaterThan(x => x.EndDate)
              .WithMessage("Document delivery date must be greater than end date.");

            RuleFor(x => x.PaymentDate)
              .NotNull().WithMessage("Document delivery date is required.")
              .GreaterThan(x => x.DocumentDeliveryDate)
              .WithMessage("Payment date must be greater than document delivery date.");
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<Period>.CreateWithOptions((Period)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
