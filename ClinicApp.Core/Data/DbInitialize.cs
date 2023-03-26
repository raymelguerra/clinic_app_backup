using ClinicApp.Core.Interfaces;
using ClinicApp.Core.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;

namespace ClinicApp.Core.Data
{
    public class DbInitializer : IDbInitialize
    {
        private readonly ClinicbdMigrationContext context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public DbInitializer(ClinicbdMigrationContext _ctx, UserManager<IdentityUser> _userManager, RoleManager<IdentityRole> _roleManager)
        {
            context = _ctx; 
            userManager = _userManager; 
            roleManager = _roleManager;
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

            if (!context.PlaceOfServices.Any())
            {
                var placeOfService = new[]
            {
                new PlaceOfService{Name="Home", Value="12"},
                new PlaceOfService{Name="School", Value="03"},
                new PlaceOfService{Name="Community", Value="99"}
            };
                foreach (PlaceOfService ps in placeOfService)
                {
                    context.PlaceOfServices.Add(ps);
                }
                context.SaveChanges();
            }

            if (!context.Procedures.Any())
            {
                var procedure = new Procedure[]
                {
                    new Procedure{Name="H2019", Rate= 19.05 },
                    new Procedure{Name="H2012", Rate= 15.24 },
                    new Procedure{Name="H2014", Rate= 12.19 },
                    new Procedure{Name="unsigned", Rate= 0 },
                };
                foreach (Procedure e in procedure)
                {
                    context.Procedures.Add(e);
                }
                context.SaveChanges();
            }

            if (!context.SubProcedures.Any())
            {
                var subProcedure = new[]
            {
                new SubProcedure{Name="unsigned", Rate= 0, ProcedureId = context.Procedures.Where(x=> x.Name == "unsigned").FirstOrDefault().Id },
                new SubProcedure{Name="97155", Rate= 19.05, ProcedureId = context.Procedures.Where(x=> x.Name == "H2019").FirstOrDefault().Id },
                new SubProcedure{Name="97156", Rate= 19.05, ProcedureId = context.Procedures.Where(x=> x.Name == "H2019").FirstOrDefault().Id },
                new SubProcedure{Name="97153", Rate= 19.05, ProcedureId = context.Procedures.Where(x=> x.Name == "H2019").FirstOrDefault().Id },
                new SubProcedure{Name="97155HN", Rate= 15.24, ProcedureId = context.Procedures.Where(x=> x.Name == "H2012").FirstOrDefault().Id },
                new SubProcedure{Name="97156HN", Rate= 15.24, ProcedureId = context.Procedures.Where(x=> x.Name == "H2012").FirstOrDefault().Id },
                new SubProcedure{Name="97153", Rate= 15.24, ProcedureId = context.Procedures.Where(x=> x.Name == "H2012").FirstOrDefault().Id },
                new SubProcedure{Name="97153", Rate= 12.19, ProcedureId = context.Procedures.Where(x=> x.Name == "H2014").FirstOrDefault().Id },
                new SubProcedure{Name="97155XP", Rate= 19.05, ProcedureId = context.Procedures.Where(x=> x.Name == "H2019").FirstOrDefault().Id },
                new SubProcedure{Name="97155XP", Rate= 19.05, ProcedureId = context.Procedures.Where(x=> x.Name == "H2012").FirstOrDefault().Id },
                new SubProcedure{Name="97153XP", Rate= 12.19, ProcedureId = context.Procedures.Where(x=> x.Name == "H2014").FirstOrDefault().Id }  
            };
                foreach (SubProcedure ps in subProcedure)
                {
                    context.SubProcedures.Add(ps);
                }
                context.SaveChanges();
            }

            if (!context.Diagnoses.Any())
            {
                var diagnosis = new Diagnosis[]
                {
                    new Diagnosis{Name="F84.0" },
                    new Diagnosis{Name="F88.0" },
                    new Diagnosis{Name="F89.0" },
                    new Diagnosis{Name="F90.0" },
                    new Diagnosis{Name="F90.1" },
                    new Diagnosis{Name="F90.2" },
                    new Diagnosis{Name="F90.8" },
                    new Diagnosis{Name="F90.9" },
                    new Diagnosis{Name="F91.0" },
                    new Diagnosis{Name="F91.1" },
                    new Diagnosis{Name="F91.2" },
                    new Diagnosis{Name="F91.3" },
                    new Diagnosis{Name="F91.8" },
                    new Diagnosis{Name="F91.9" },
                    new Diagnosis{Name="F98.0" },
                    new Diagnosis{Name="F98.1" },
                    new Diagnosis{Name="F98.2" },
                    new Diagnosis{Name="F98.21" },
                    new Diagnosis{Name="F98.29" },
                    new Diagnosis{Name="F98.3" },
                    new Diagnosis{Name="F98.4" },
                    new Diagnosis{Name="F98.5" },
                    new Diagnosis{Name="F98.8" },
                    new Diagnosis{Name="F98.9" },
                    new Diagnosis{Name="Q90.9" },
                    new Diagnosis{Name="Q90.0" },
                    new Diagnosis{Name="Q90.1" },
                    new Diagnosis{Name="Q90.2" },
                    new Diagnosis { Name = "Q84" },
                    new Diagnosis { Name = "R46.89" },
                    new Diagnosis { Name = "R62.50" }

                };
                foreach (Diagnosis d in diagnosis)
                {
                    context.Diagnoses.Add(d);
                }

                context.SaveChanges();
            }

            if (!context.ReleaseInformations.Any())
            {
                var releases = new ReleaseInformation[]
                {
                    new ReleaseInformation{Name="INFORMED CONSENT TO RELEASE BY FEDERAL STATUTES" }
                };
                foreach (ReleaseInformation d in releases)
                {
                    context.ReleaseInformations.Add(d);
                }
                context.SaveChanges();
            }

            if (!context.Companies.Any())
            {
                var companies = new Company[]
                {
                    new Company{Name="Expanding Possibilities", Acronym="EP" },
                    new Company{Name="Villa Lyan", Acronym="VL" }
                };
                foreach (Company d in companies)
                {
                    context.Companies.Add(d);
                }
                context.SaveChanges();
            }


            var rol1 = roleManager.CreateAsync(new IdentityRole {Name="Administrator", NormalizedName="ADMINISTRATOR" }).Result;
            var rol2 = roleManager.CreateAsync(new IdentityRole {Name="Operator", NormalizedName="OPERATOR" }).Result;
            var rol3 = roleManager.CreateAsync(new IdentityRole { Name = "Biller", NormalizedName = "BILLER" }).Result;
            var rol4 = roleManager.CreateAsync(new IdentityRole { Name = "Contractor", NormalizedName = "CONTRACTOR" }).Result;




        }
    }
}