using ClinicApp.Core.Models;
using FluentValidation;
using MediatR;

namespace ClinicApp.WebApp.Services.Validations;

public class UnitValidator : AbstractValidator<UnitDetail>
{
    public UnitValidator()
    {
        RuleFor(sl => sl.Procedure)
                             .NotNull().WithMessage("Procedure cannot be null.");
        RuleFor(sl => sl.DateOfService)
            .NotNull().WithMessage("Date Of Service cannot be null.")
            .LessThan(DateTime.Now).WithMessage("Date of Service must be before today");
        RuleFor(sl => sl.Unit)
            .NotNull().WithMessage("Unit cannot be null.")
            .InclusiveBetween(1,24).WithMessage("Unit must be less than 24 and greater than 0 units");
        RuleFor(sl => sl.PlaceOfService)
            .NotNull().WithMessage("Place of Service cannot be null.");
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<UnitDetail>.CreateWithOptions((UnitDetail)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}