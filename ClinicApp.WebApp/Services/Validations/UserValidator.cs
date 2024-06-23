using ClinicApp.Core.Dtos;
using ClinicApp.Core.Models;
using FluentValidation;

namespace ClinicApp.WebApp.Services.Validations
{
    public class UserValidator : AbstractValidator<UserVM>
    {
        public UserValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("The 'Name' field is required.")
                .NotNull().WithMessage("The 'Name' field cannot be null.")
                .Length(1, 100).WithMessage("The 'Name' field must be between 1 and 100 characters.");
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("The 'Email' field is required.")
                .NotNull().WithMessage("The 'Email' field cannot be null.")
                .EmailAddress().WithMessage("The 'Email' field is not a valid e-mail address.")
                .Length(1, 100).WithMessage("The 'Email' field must be between 1 and 100 characters.");

            RuleFor(x => x.SurName).NotNull().WithMessage("The 'SurName' field cannot be null.");
            RuleFor(x => x.Roles).NotNull().WithMessage("The 'Roles' field cannot be null.");

        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<UserVM>.CreateWithOptions((UserVM)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
