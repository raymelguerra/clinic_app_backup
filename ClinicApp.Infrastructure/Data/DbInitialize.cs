using ClinicApp.Infrastructure.Interfaces;
using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Persistence;


namespace ClinicApp.Infrastructure.Data;

public class DbInitializer : IDbInitialize
{
    private readonly InsuranceContext context;

    public DbInitializer(InsuranceContext _ctx)
    {
        context = _ctx;
    }
    public void Initialize()
    {
        // Console.WriteLine(context.Database.)
        context.Database.EnsureCreated();

        // Look for any Contractor type.
        if (!context.ContractorTypes.Any())
        {
            var contractorTypes = new ContractorType[]
            {
                new ContractorType{Name="Analyst"},
                new ContractorType{Name= "RBT"},
            };

            foreach (ContractorType ct in contractorTypes)
            {
                context.ContractorTypes.Add(ct);
            }
            context.SaveChanges();
        }

        if (!context.PlacesOfService.Any())
        {
            var placeOfService = new[]
        {
            new PlaceOfService{Name="Home", Value="12"},
            new PlaceOfService{Name="School", Value="03"},
            new PlaceOfService{Name="Community", Value="99"}
        };
            foreach (PlaceOfService ps in placeOfService)
            {
                context.PlacesOfService.Add(ps);
            }
            context.SaveChanges();
        }

        if (!context.Procedures.Any())
        {
            var procedure = new Procedure[]
            {
                new Procedure{Name="97155", ContractorTypeId=1},
                new Procedure{Name="97156", ContractorTypeId = 1},
                new Procedure{Name="97155HN", ContractorTypeId = 1},
                new Procedure{Name="97156HN", ContractorTypeId = 1},
                new Procedure{Name="97153", ContractorTypeId=1},
                new Procedure{Name="97153", ContractorTypeId=2},
                new Procedure{Name="97155XP", ContractorTypeId = 1},
                new Procedure{Name="97153XP", ContractorTypeId = 2},
                new Procedure{Name="97151", ContractorTypeId = 1},
                new Procedure{Name="97152", ContractorTypeId = 1},
                new Procedure{Name="97151TS", ContractorTypeId = 1},
            };
            foreach (Procedure e in procedure)
            {
                context.Procedures.Add(e);
            }
            context.SaveChanges();
        }


        if (!context.Diagnoses.Any())
        {
            var diagnosis = new Diagnosis[]
            {
                new Diagnosis{Name="F90.1"},
                new Diagnosis{Name="Q90.1"},
                new Diagnosis{Name="Q90.0"},
                new Diagnosis{Name="Q90.9"},
                new Diagnosis{Name="F98.9"},
                new Diagnosis{Name="F98.8"},
                new Diagnosis{Name="F98.5"},
                new Diagnosis{Name="F98.4"},
                new Diagnosis{Name="F98.3"},
                new Diagnosis{Name="F98.29"},
                new Diagnosis{Name="F98.21"},
                new Diagnosis{Name="F98.2"},
                new Diagnosis{Name="Q90.2"},
                new Diagnosis{Name="F98.1"},
                new Diagnosis{Name="F91.9"},
                new Diagnosis{Name="F91.8"},
                new Diagnosis{Name="F91.3"},
                new Diagnosis{Name="F91.2"},
                new Diagnosis{Name="F91.1"},
                new Diagnosis{Name="F91.0"},
                new Diagnosis{Name="F90.9"},
                new Diagnosis{Name="F90.8"},
                new Diagnosis{Name="F90.2"},
                new Diagnosis{Name="Q84.0"},
                new Diagnosis{Name="F90.0"},
                new Diagnosis{Name="F98.0"},
                new Diagnosis{Name="F84.0"},
                new Diagnosis{Name="F89.0"},
                new Diagnosis{Name="F80.1"},
                new Diagnosis{Name="F80.89"},
                new Diagnosis{Name="R46.89"},
                new Diagnosis{Name="Z72.89"},
                new Diagnosis{Name="R62.50"},
                new Diagnosis{Name="F88"}
            };
            foreach (Diagnosis d in diagnosis)
            {
                context.Diagnoses.Add(d);
            }

            context.SaveChanges();
        }

        if (!context.ReleaseInformations.Any())
        {
            var release = new ReleaseInformation { Name = "INFORMED CONSENT TO RELEASE BY FEDERAL STATUTES" };
            context.ReleaseInformations.Add(release);
            context.SaveChanges();
        }

        if (!context.Companies.Any())
        {
            var company = new Company { Name = "Expanding Possibilities", Acronym = "EP" };

            context.Companies.Add(company);
            context.SaveChanges();
        }

    }
}