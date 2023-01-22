using ClinicApp.Core.Data;
using ClinicApp.Core.DTO;
using ClinicApp.Core.Models;
using ClinicApp.MSDashboard.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace ClinicApp.MSDashboard.Services
{
    public class DashboardService : IDashboard
    {
        private readonly ClinicbdMigrationContext _context;
        public DashboardService(ClinicbdMigrationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Company>> GetCompanies()
        {
            return await _context.Companies.ToListAsync();
        }

        public async Task<GeneralData> GetGeneralData(int company_id, int period_id)
        {
            return new GeneralData
            {
                Client = await _context.Clients.Where(x => x.Agreements.Any(a => a.CompanyId == company_id)).CountAsync(),
                Contractor = await _context.Contractors.Where(x => x.Payrolls.Any(p => p.CompanyId == company_id)).CountAsync(),
                ServiceLog = await _context.ServiceLogs.Where(x => x.PeriodId == period_id && x.Client.Agreements.Any(a => a.CompanyId == company_id) && x.Contractor.Payrolls.Any(p => p.CompanyId == company_id)).CountAsync()
            };
        }

        public async Task<IEnumerable<Period>> GetPeriods()
        {
            var periodQry = await _context.Periods
              .Where(p => p.StartDate < DateTime.Now)
              .OrderByDescending(p => p.StartDate)
              .ToListAsync();

            return periodQry;
        }

        public async Task<IEnumerable<ProfitHistory>> GetProfit(int company_id)
        {
                var sqlQuery = $"select \"PayPeriod\", " +
                   $"round(cast(sum((\"Unit\") * CASE WHEN sp.\"Id\" = 1 then pro.\"Rate\" ELSE sp.\"Rate\" END) as numeric),2) as Billed, " +
                   $"round(cast(sum((\"Unit\"/4) * ag.\"RateEmployees\") as numeric),2) as Payment, " +
                   $"round(cast(sum((\"Unit\") * CASE WHEN sp.\"Id\" = 1 then pro.\"Rate\" ELSE sp.\"Rate\" END) as numeric),2) - " +
                   $"round(cast(sum((\"Unit\"/4) * ag.\"RateEmployees\") as numeric),2) as Profit " +
                   $"from \"Agreement\" ag " +
                   $"inner join \"Payroll\" pr on ag.\"PayrollId\" = pr.\"Id\" and pr.\"CompanyId\" = ag.\"CompanyId\" " +
                   $"inner join \"Procedure\" pro on pro.\"Id\" = pr.\"ProcedureId\" " +
                   $"inner join \"ServiceLog\" sl on sl.\"ClientId\" = ag.\"ClientId\" and pr.\"ContractorId\" = sl.\"ContractorId\" " +
                   $"inner join \"Period\" p on p.\"Id\" = sl.\"PeriodId\" " +
                   $"inner join \"UnitDetail\" ud on sl.\"Id\" = ud.\"ServiceLogId\" " +
                   $"inner join \"SubProcedure\" sp on ud.\"SubProcedureId\" = sp.\"Id\" " +
                   $"WHERE ag.\"CompanyId\" = {company_id} " +
                   $"group by p.\"Id\", p.\"PayPeriod\" " +
                   $"order by p.\"Id\";";

            return await _context.ProfitHistory!.FromSqlRaw(sqlQuery).ToListAsync();
        }

        public async Task<IEnumerable<ServiceLogWithoutPatientAccount>> GetServiceLogWithoutPatientAccount(int company_id, int period_id)
        {
            var sqlQuery = $"SELECT cl.\"Id\" ClientId, cl.\"Name\" Client, ct.\"Id\", ct.\"Name\" Contractor, " +
               $"ud.\"DateOfService\", ud.\"SubProcedureId\" SubProcedureId, sp.\"Name\" \"Procedure\" " +
               $"FROM \"Agreement\" ag " +
               $"INNER JOIN \"Company\" c ON c.\"Id\" = ag.\"CompanyId\" AND c.\"Id\" = {company_id} " +
               $"INNER JOIN \"Client\" cl ON cl.\"Id\" = ag.\"ClientId\" " +
               $"INNER JOIN \"Payroll\" pr ON pr.\"Id\" = ag.\"PayrollId\" " +
               $"INNER JOIN \"Contractor\" ct ON ct.\"Id\" = pr.\"ContractorId\" " +
               $"INNER JOIN \"ServiceLog\" sl ON sl.\"ClientId\" = cl.\"Id\" AND sl.\"ContractorId\" = ct.\"Id\" AND sl.\"PeriodId\"= {period_id} " +
               $"INNER JOIN \"UnitDetail\" ud ON sl.\"Id\" = ud.\"ServiceLogId\" " +
               $"INNER JOIN \"SubProcedure\" sp ON sp.\"Id\" = ud.\"SubProcedureId\" " +
               $"left JOIN \"PatientAccount\" pa ON pa.\"ClientId\" = cl.\"Id\" AND ud.\"DateOfService\" BETWEEN pa.\"CreateDate\" AND pa.\"ExpireDate\" " +
               $"and coalesce(CASE WHEN(sp.\"Name\" LIKE '%51' OR sp.\"Name\" LIKE '%51TS') " +
               $"THEN pa.\"Auxiliar\" ELSE pa.\"LicenseNumber\" END, 'DOES NOT APPLY') <> 'DOES NOT APPLY' " +
               $"WHERE pa.\"Id\" is null " +
               $"ORDER BY cl.\"Name\", ct.\"Name\", ud.\"DateOfService\";";

            return await _context.ServiceLogWithoutPatientAccount!.FromSqlRaw(sqlQuery).ToListAsync();
        }

        public async Task<ServicesLogStatus> GetServicesLgStatus(int company_id, int period_id)
        {
            var sqlQuery = $"SELECT SUM(CASE WHEN \"Biller\" IS NOT NULL THEN 1 ELSE 0 END) AS Billed, " +
                $"SUM(CASE WHEN \"Pending\" IS NOT NULL THEN 1 ELSE 0 END) AS Pending, " +
                $"SUM(CASE WHEN \"Biller\" IS NULL AND \"Pending\" IS NULL THEN 1 ELSE 0 END) AS NotBilled " +
                $"from \"Agreement\" ag " +
                $"inner join \"Payroll\" pr on ag.\"PayrollId\" = pr.\"Id\" and pr.\"CompanyId\" = ag.\"CompanyId\" " +
                $"inner join \"ServiceLog\" sl on sl.\"ClientId\" = ag.\"ClientId\" and pr.\"ContractorId\" = sl.\"ContractorId\" " +
                $"inner join \"Period\" p on p.\"Id\" = sl.\"PeriodId\" " +
                $"WHERE sl.\"PeriodId\"= {period_id} " +
                $"and  ag.\"CompanyId\"= {company_id}; ";

            var result = await _context.ServicesLogStatus!.FromSqlRaw(sqlQuery).ToListAsync();
            return result.FirstOrDefault();
        }
    }
}
