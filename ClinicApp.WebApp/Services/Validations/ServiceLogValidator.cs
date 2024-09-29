using ClinicApp.Core.Models;
using FluentValidation;
using Oauth2.sdk.Models;

namespace ClinicApp.WebApp.Services.Validations
{
    public class ServiceLogValidator : AbstractValidator<ServiceLog>
    {
        public ServiceLogValidator()
        {
            RuleFor(x => x.Period)
                 .NotEmpty().WithMessage("Period is required.");

            RuleFor(x => x.Contractor)
                .NotEmpty().WithMessage("Contractor is required.");

            RuleFor(x => x.Client)
                .NotEmpty().WithMessage("Client is required.");

            RuleFor(x => x.Insurance)
                .NotEmpty().WithMessage("Insurance is required.");

            RuleFor(x => x.UnitDetails)
                .Must(x => x != null && x.Count > 0).WithMessage("At least one Unit Detail must be provided.")
                .ForEach(slRule =>
                {
                    slRule.ChildRules(unit =>
                    {
                        unit.RuleFor(sl => sl.Procedure)
                            .NotNull().WithMessage("Procedure cannot be null.");
                        unit.RuleFor(sl => sl.DateOfService)
                            .NotNull().WithMessage("Date Of Service cannot be null.")
                            .LessThan(DateTime.Now).WithMessage("Date of Service must be before today");
                        unit.RuleFor(sl => sl.Unit)
                            .NotNull().WithMessage("Unit cannot be null.")
                            .LessThan(24).WithMessage("Unit must be less than 24 units");

                    });
                });

            //RuleFor(x => x.UnitDetails)
            //   .Custom((unitDetails, context) =>
            //   {
            //       var totalHoursAnalyst = unitDetails
            //           .Where(detail => contractor.Payrolls.Any(p => p.ContractorType.Name == "Analyst") &&
            //                  detail.Procedure != null && detail.Procedure.Name.Contains("XP"))
            //           .Sum(detail => detail.Unit * 0.25); // 1 unidad = 1/4 hora

            //       var totalHoursRBT = unitDetails
            //           .Where(detail => contractor.Payrolls.Any(p => p.ContractorType.Name == "RBT") &&
            //                  detail.Procedure != null && detail.Procedure.Name.Contains("XP"))
            //           .Sum(detail => detail.Unit * 0.25); // 1 unidad = 1/4 hora

            //       // Validar según el tipo de contratista en cada payroll
            //       foreach (var payroll in contractor.Payrolls)
            //       {
            //           if (payroll.ContractorType.Name == "Analyst")
            //           {
            //               if (totalHoursAnalyst > client.WeeklyApprovedAnalyst)
            //               {
            //                   context.AddFailure($"Total hours cannot exceed {client.WeeklyApprovedAnalyst} hours for Analysts.");
            //               }
            //           }
            //           else if (payroll.ContractorType.Name == "RBT")
            //           {
            //               if (totalHoursRBT > client.WeeklyApprovedRBT)
            //               {
            //                   context.AddFailure($"Total hours cannot exceed {client.WeeklyApprovedRBT} hours for RBTs.");
            //               }
            //           }
            //       }
            //   });
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
