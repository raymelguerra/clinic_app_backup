﻿using ClinicApp.Core.Models;
using FluentValidation;

namespace ClinicApp.WebApp.Services.Validations
{
    public class PhysicianValidator : AbstractValidator<Contractor>
    {
        public PhysicianValidator()
        {
            RuleFor(x => x.Extra)
            .Length(1, 100).WithMessage("The 'Extra' field must be between 1 and 100 characters.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("The 'Name' field is required.")
                .NotNull().WithMessage("The 'Name' field cannot be null.")
                .Length(1, 100).WithMessage("The 'Name' field must be between 1 and 100 characters.");

            RuleFor(x => x.RenderingProvider)
                .NotEmpty().WithMessage("The 'RenderingProvider' field is required.")
                .NotNull().WithMessage("The 'RenderingProvider' field cannot be null.")
                .Length(1, 100).WithMessage("The 'RenderingProvider' field must be between 1 and 100 characters.");

            RuleFor(x => x.Payrolls)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("The 'Payrolls' list cannot be null.")
                .NotEmpty().WithMessage("The 'Payrolls' list cannot be empty.")
                .Must(items => items != null && items.Any()).WithMessage("The 'Payrolls' list must contain at least one element.");
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<Contractor>.CreateWithOptions((Contractor)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
