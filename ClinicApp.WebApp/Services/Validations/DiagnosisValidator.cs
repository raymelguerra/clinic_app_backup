using ClinicApp.Core.Models;
using FluentValidation;

namespace ClinicApp.WebApp.Services.Validations
{
    public class DiagnosisValidator : AbstractValidator<Diagnosis>
    {
        public DiagnosisValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("The 'Name' field is required.")
                .NotNull().WithMessage("The 'Name' field cannot be null.")
                .Length(1, 100).WithMessage("The 'Name' field must be between 1 and 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("The 'Description' field is required.")
                .NotNull().WithMessage("The 'Description' field cannot be null.");            
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<Diagnosis>.CreateWithOptions((Diagnosis)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
