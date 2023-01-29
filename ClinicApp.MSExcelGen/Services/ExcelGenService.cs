using ClinicApp.Core.Data;
using ClinicApp.Core.DTO;
using ClinicApp.Core.Models;
using ClinicApp.MSExcelGen.Dtos;
using ClinicApp.MSExcelGen.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicApp.MSExcelGen.Services
{

    public class ExcelGenService : IExcelGen
    {
        private readonly IConfiguration _config;
        private readonly ClinicbdMigrationContext _context;
        public ExcelGenService(ClinicbdMigrationContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<List<AgreementDto>> GetAgreementsAsync(int ContractorId, int CompanyId)
        {
            var agreementQry = await (from ag in _context.Agreements
                                      where ag.Payroll.ContractorId == ContractorId && ag.CompanyId == CompanyId
                                      //orderby ag.ClientId, ag.Payroll.ContractorTypeId
                                      select new AgreementDto
                                      {
                                          Id = ag.Id,
                                          ClientId = ag.ClientId,                                                                                    
                                          RateEmployees = ag.RateEmployees,
                                          CompanyId = ag.CompanyId,
                                          Company = new CompanyDto()
                                          {
                                              Id = ag.Company.Id,
                                              Name = ag.Company.Name,
                                              Acronym = ag.Company.Acronym
                                          },
                                          PayrollId = ag.PayrollId,
                                          Payroll = new PayrollDto()
                                          {
                                              Id = ag.Payroll.Id,
                                              ContractorId = ag.Payroll.ContractorId,
                                              Contractor = new ContractorDto()
                                              {
                                                  Id = ag.Payroll.Contractor.Id,
                                                  Name = ag.Payroll.Contractor.Name,
                                                  Extra = ag.Payroll.Contractor.Extra,
                                                  RenderingProvider = ag.Payroll.Contractor.RenderingProvider
                                              },
                                              ContractorTypeId = ag.Payroll.ContractorTypeId,
                                              ContractorType = new ContractorTypeDto()
                                              {
                                                  Id = ag.Payroll.ContractorType.Id,
                                                  Name = ag.Payroll.ContractorType.Name,
                                              },
                                              ProcedureId = ag.Payroll.ProcedureId,
                                              Procedure = new ProcedureDto()
                                              {
                                                  Id = ag.Payroll.Procedure.Id,
                                                  Name = ag.Payroll.Procedure.Name,
                                                  Rate = ag.Payroll.Procedure.Rate,
                                              }
                                          }
                                      }).ToListAsync();
            return agreementQry;
        }

        public async Task<List<CompanyDto>> GetCompanies()
        {
            var companyQry = await (from c in _context.Companies
                                    select new CompanyDto()
                                    {
                                        Id = c.Id,
                                        Name = c.Name,
                                        Acronym = c.Acronym
                                    }).ToListAsync();

            return companyQry;
        }

        public async Task<List<ExtendedAgrUnitDetail>> GetExAgrUnitDetails(int ClientId, int ContractorId, int CompanyId, int PeriodId)
        {
            var unitDetQry = await (from ag in _context.Agreements
                                    join pr in _context.Payrolls on ag.PayrollId equals pr.Id
                                    join sl in _context.ServiceLogs on new { ag.ClientId, pr.ContractorId } equals new { sl.ClientId, sl.ContractorId }
                                    join ud in _context.UnitDetails on sl.Id equals ud.ServiceLogId
                                    where sl.ClientId == ClientId &&
                                      ((pr.ContractorTypeId == 1 && sl.ContractorId == ContractorId) ||
                                       (pr.ContractorTypeId != 1 && !(from inag in _context.Agreements where inag.CompanyId == CompanyId && inag.ClientId == ClientId && inag.Payroll.ContractorId < ContractorId && inag.Payroll.ContractorTypeId == 1 select inag).Any())) &&
                                      ag.CompanyId == CompanyId &&
                                      sl.PeriodId == PeriodId
                                    //sl.CreatedDate > DbFunctions.AddDays(previousPeriod.DocumentDeliveryDate, 2) &&
                                    //sl.CreatedDate <= DbFunctions.AddDays(period.DocumentDeliveryDate, 2)                                   
                                    orderby sl.ClientId, pr.ContractorTypeId, sl.ContractorId, ud.SubProcedureId, ud.DateOfService
                                    select new ExtendedAgrUnitDetail
                                    {
                                        serviceLog = new ServiceLogDto()
                                        {
                                            Id = sl.Id,
                                            ClientId = sl.ClientId,
                                            ContractorId = sl.ContractorId,
                                            PeriodId = sl.PeriodId,
                                            CreatedDate = sl.CreatedDate,
                                            BilledDate = sl.BilledDate,
                                            Biller = sl.Biller,
                                            Pending = sl.Pending,
                                            Client = new ClientDto()
                                            {
                                                Id = sl.Client.Id,
                                                Name = sl.Client.Name,
                                                AuthorizationNumber = sl.Client.AuthorizationNumber,
                                                PatientAccount = sl.Client.PatientAccount,
                                                RecipientId = sl.Client.RecipientId,
                                                Enabled = sl.Client.Enabled,
                                                Sequence = sl.Client.Sequence,
                                                WeeklyApprovedAnalyst = sl.Client.WeeklyApprovedAnalyst,
                                                WeeklyApprovedRbt = sl.Client.WeeklyApprovedRbt,
                                                ReferringProvider = sl.Client.ReferringProvider,
                                                DiagnosisId = sl.Client.DiagnosisId,
                                                ReleaseInformationId = sl.Client.ReleaseInformationId,
                                                Diagnosis = new DiagnosisDto()
                                                {
                                                    Id = sl.Client.Diagnosis.Id,
                                                    Name = sl.Client.Diagnosis.Name,
                                                    Description = sl.Client.Diagnosis.Description,
                                                },
                                                ReleaseInformation = new ReleaseInformationDto()
                                                {
                                                    Id = sl.Client.ReleaseInformation.Id,
                                                    Name = sl.Client.ReleaseInformation.Name
                                                }
                                            },
                                            Contractor = new ContractorDto()
                                            {
                                                Id = sl.Contractor.Id,
                                                Name = sl.Contractor.Name,
                                                Extra = sl.Contractor.Extra,
                                                RenderingProvider = sl.Contractor.RenderingProvider
                                            },
                                        },
                                        unitDetail = new UnitDetailDto()
                                        {
                                            Id = ud.Id,
                                            Modifiers = ud.Modifiers,
                                            DateOfService = ud.DateOfService,
                                            Unit = ud.Unit,
                                            PlaceOfServiceId = ud.PlaceOfServiceId,
                                            PlaceOfService = new PlaceOfServiceDto()
                                            {
                                                Id = ud.PlaceOfService.Id,
                                                Name = ud.PlaceOfService.Name,
                                                Value = ud.PlaceOfService.Value,
                                            },
                                            ServiceLogId = ud.ServiceLogId,
                                            ServiceLog = null, //No estoy seguro de esto
                                            SubProcedureId = ud.SubProcedureId,
                                            SubProcedure = new SubProcedureDto()
                                            {
                                                Id = ud.SubProcedure.Id,
                                                Name = ud.SubProcedure.Name,
                                                Rate = ud.SubProcedure.Rate,
                                            }

                                        },
                                        agreement = new AgreementDto
                                        {
                                            Id = ag.Id,
                                            ClientId = ag.ClientId,
                                            CompanyId = ag.CompanyId,
                                            PayrollId = ag.PayrollId,
                                            RateEmployees = ag.RateEmployees,
                                            Company = new CompanyDto()
                                            {
                                                Id = ag.Company.Id,
                                                Name = ag.Company.Name,
                                                Acronym = ag.Company.Acronym
                                            },
                                            Payroll = new PayrollDto()
                                            {
                                                Id = ag.Payroll.Id,
                                                ContractorId = ag.Payroll.ContractorId,
                                                Contractor = new ContractorDto()
                                                {
                                                    Id = ag.Payroll.Contractor.Id,
                                                    Name = ag.Payroll.Contractor.Name,
                                                    Extra = ag.Payroll.Contractor.Extra,
                                                    RenderingProvider = ag.Payroll.Contractor.RenderingProvider
                                                },
                                                ContractorTypeId = ag.Payroll.ContractorTypeId,
                                                ContractorType = new ContractorTypeDto()
                                                {
                                                    Id = ag.Payroll.ContractorType.Id,
                                                    Name = ag.Payroll.ContractorType.Name,
                                                },
                                                ProcedureId = ag.Payroll.ProcedureId,
                                                Procedure = new ProcedureDto()
                                                {
                                                    Id = ag.Payroll.Procedure.Id,
                                                    Name = ag.Payroll.Procedure.Name,
                                                    Rate = ag.Payroll.Procedure.Rate,
                                                }
                                            }
                                        }
                                    }).ToListAsync();
            return unitDetQry;
        }

        public async Task<List<ExtendedContractor>> GetExContractorsAsync(string companyCode)
        {
            var contractorsQry = await (from ag in _context.Agreements
                                        where ag.Payroll.ContractorTypeId == 1 && (companyCode == "" || ag.Company.Acronym == companyCode)
                                        select new ExtendedContractor {
                                            contractorId = ag.Payroll.ContractorId,
                                            companyId = ag.CompanyId
                                        }).Distinct().ToListAsync();

            return contractorsQry;
        }

        public async Task<ExtendedPeriod> GetPeriodAsync(int PeriodId = -1)
        {
            return await (from p in _context.Periods!
                          where (PeriodId == -1 && p.EndDate < DateTime.Now) || (PeriodId != -1 && p.Id == PeriodId)
                          orderby p.StartDate descending
                          select new ExtendedPeriod() {
                              Id = p.Id,
                              StartDate = p.StartDate,
                              EndDate = p.EndDate,
                              PayPeriod = p.PayPeriod
                          }).Take(1).FirstOrDefaultAsync();
        }

        public async Task<List<ExtendedPeriod>> GetPeriodsAsync()
        {
            return await (from p in _context!.Periods!
                          where (p.StartDate < DateTime.Now)
                          orderby p.StartDate descending
                          select new ExtendedPeriod {
                              Id = p.Id,
                              StartDate = p.StartDate,
                              EndDate = p.EndDate,
                              PayPeriod = p.PayPeriod
                          })
                         .ToListAsync();
        }
    }
}
