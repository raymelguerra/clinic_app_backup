using ClinicApp.Core.Data;
using ClinicApp.Core.DTO;
using ClinicApp.Core.Models;
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

        public async Task<List<Agreement>> GetAgreementsAsync(int ContractorId, int CompanyId)
        {
            var agreementQry = await (from ag in _context.Agreements
                                where ag.Payroll.ContractorId == ContractorId && ag.CompanyId == CompanyId
                                select ag).ToListAsync();

            return agreementQry;
        }

        public async Task<List<Company>> GetCompanies()
        {
            var companyQry = await (from c in _context.Companies
                             select c).ToListAsync();

            return companyQry;
        }

        public async Task<List<ExtendedAgrUnitDetail>> GetExAgrUnitDetails(int ClientId, int ContractorId, int CompanyId, int PeriodId)
        {
            var unitDetQry = await (from ag in _context.Agreements
                             join pr in _context.Payrolls on ag.PayrollId equals pr.Id
                             join sl in _context.ServiceLogs on new { ag.ClientId, pr.ContractorId } equals new { sl.ClientId, sl.ContractorId }
                             join ud in _context.UnitDetails on sl.Id equals ud.ServiceLogId
                             where sl.ClientId == ClientId &&
                               ((ag.Payroll.ContractorTypeId == 1 && sl.ContractorId == ContractorId) ||
                                (ag.Payroll.ContractorTypeId != 1 && !(from inag in _context.Agreements where inag.CompanyId == CompanyId && inag.ClientId == ClientId && inag.Payroll.ContractorId < ContractorId && inag.Payroll.ContractorTypeId == 1 select inag).Any())) &&
                               ag.CompanyId == CompanyId &&
                               sl.PeriodId == PeriodId
                             //sl.CreatedDate > DbFunctions.AddDays(previousPeriod.DocumentDeliveryDate, 2) &&
                             //sl.CreatedDate <= DbFunctions.AddDays(period.DocumentDeliveryDate, 2)                                   
                             orderby sl.ClientId, ag.Payroll.ContractorTypeId, sl.ContractorId, ud.SubProcedureId, ud.DateOfService
                             select new ExtendedAgrUnitDetail
                             {
                                 serviceLog = sl,
                                 unitDetail = ud,
                                 agreement = ag
                             }).ToListAsync();
            return unitDetQry;
        }

        public async Task<List<ExtendedContractor>> GetExContractorsAsync(string companyCode)
        {
            var contractorsQry = await (from ag in _context.Agreements
                                  where ag.Payroll.ContractorTypeId == 1 && (companyCode == "" | ag.Company.Acronym == companyCode)
                                  select new ExtendedContractor { contractor = ag.Payroll.Contractor, company = ag.Company }).Distinct().ToListAsync();

            return contractorsQry;
        }

        public async Task<Period> GetPeriodAsync(int PeriodId = -1)
        {
            return await (from p in _context.Periods!
                             where (PeriodId == -1 && p.EndDate < DateTime.Now) || (PeriodId != -1 && p.Id == PeriodId)
                             orderby p.StartDate descending
                             select p).Take(1).FirstOrDefaultAsync();
        }

        public async Task<List<ExtendedPeriod>> GetPeriodsAsync()
        {
            return await (from p in _context!.Periods!
                         where (p.StartDate < DateTime.Now)
                         orderby p.StartDate descending
                         select new ExtendedPeriod { Id = p.Id, StartDate = p.StartDate, EndDate = p.EndDate, PayPeriod = p.PayPeriod })
                         .ToListAsync();
        }
    }
}
