using ClinicApp.Core.Models;
using FluentValidation;

namespace ClinicApp.WebApp.Services.Validations;

public class PatientValidator : AbstractValidator<Client>
{
    public PatientValidator()
    {
        RuleFor(client => client.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot be more than 100 characters.");

        RuleFor(client => client.RecipientId)
            .NotEmpty().WithMessage("Recipient ID is required.")
            .MaximumLength(50).WithMessage("Recipient ID cannot be more than 50 characters.");

        RuleFor(client => client.PatientAccount)
            .NotEmpty().WithMessage("Patient account is required.")
            .MaximumLength(50).WithMessage("Patient account cannot be more than 50 characters.");

        RuleFor(client => client.ReferringProvider)
            .NotEmpty().WithMessage("Referring provider is required.")
            .MaximumLength(100).WithMessage("Referring provider cannot be more than 100 characters.");

        RuleFor(client => client.AuthorizationNumber)
            .NotEmpty().WithMessage("Authorization number is required.")
            .MaximumLength(50).WithMessage("Authorization number cannot be more than 50 characters.");

        RuleFor(client => client.Sequence)
            .GreaterThan(0).WithMessage("Sequence must be greater than zero.");

        RuleFor(client => client.Enabled)
            .NotNull().WithMessage("Enabled property cannot be null.");

        RuleFor(client => client.WeeklyApprovedRBT)
            .GreaterThanOrEqualTo(0).WithMessage("WeeklyApprovedRbt must be greater or equal to zero.");

        RuleFor(client => client.WeeklyApprovedAnalyst)
            .GreaterThanOrEqualTo(0).WithMessage("WeeklyApprovedAnalyst must be greater or equal to zero.");

        RuleFor(client => client.Agreements)
            .NotEmpty().WithMessage("At least one agreement must be associated.");

        RuleFor(client => client.Diagnosis)
            .NotNull().WithMessage("Diagnosis cannot be null.");

        RuleFor(client => client.PatientAccounts)
            .NotEmpty().WithMessage("At least one patient account must be associated.");

        RuleFor(client => client.ReleaseInformation)
            .NotNull().WithMessage("Release information cannot be null.");

    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<Client>.CreateWithOptions((Client)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}
