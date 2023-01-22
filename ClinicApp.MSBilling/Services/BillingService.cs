using ClinicApp.Core.Data;
using ClinicApp.Core.DTO;
using ClinicApp.Core.Models;
using ClinicApp.MSBilling.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicApp.MSBilling.Services;

public class BillingService: IBilling
{
    private readonly IConfiguration _config;
    private readonly ClinicbdMigrationContext _context;
    public BillingService(ClinicbdMigrationContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<List<ExtendedPeriod>> GetPeriodsAsync()
    {
        var periodQry = _context.Periods
            .Where(p => p.StartDate < DateTime.Now)
            .OrderByDescending(p => p.StartDate)
            .Select(p => new ExtendedPeriod { Id = p.Id, StartDate = p.StartDate, EndDate = p.EndDate, PayPeriod = p.PayPeriod });

        return await periodQry.ToListAsync();
    }

    public async Task<Period> GetPeriodAsync(int periodID)
    {
        var infoPeriod = (from p in _context.Periods
                          where p.Id == periodID
                          select p).Take(1);
        return await infoPeriod.SingleOrDefaultAsync();
    }


    public async Task<List<Company>> GetCompaniesAsync()
    {
        var companyQry = from c in _context.Companies
                         select c;

        return await companyQry.ToListAsync();
    }

    public async Task<List<TvClient>> GetContractorAndClientsAsync(string CompanyCode, int PeriodId)
    {
        _context.ChangeTracker.LazyLoadingEnabled = false;

        var sufixList = _config.GetValue<string>("ExtraProceduresList") + ";";

        var queryRes = await (from ag in _context.Agreements
                              join co in _context.Companies on new { ag.CompanyId, CompanyCode } equals new { CompanyId = co.Id, CompanyCode = co.Acronym }
                              join pr in _context.Payrolls on ag.PayrollId equals pr.Id
                              join ctt in _context.ContractorTypes on pr.ContractorTypeId equals ctt.Id
                              join ct in _context.Contractors on pr.ContractorId equals ct.Id
                              join cl in _context.Clients on ag.ClientId equals cl.Id
                              join pa in _context.PatientAccounts on ag.ClientId equals pa.ClientId
                              join sl in _context.ServiceLogs on new { ag.ClientId, pr.ContractorId, PeriodId } equals new { sl.ClientId, sl.ContractorId, sl.PeriodId }
                              join ud in _context.UnitDetails on sl.Id equals ud.ServiceLogId
                              join sp in _context.SubProcedures on ud.SubProcedureId equals sp.Id
                              where pa.CreateDate <= ud.DateOfService && pa.ExpireDate >= ud.DateOfService
                              && (((sufixList.Contains(sp.Name.Substring(3) + ";") ? pa.Auxiliar : pa.LicenseNumber) ?? "DOES NOT APPLY") != "DOES NOT APPLY")
                              select new { cl, ct, ctt, pa, sl, ud, sp })
                                  .Distinct()
                                  .OrderBy(it => it.cl.Name.Trim())
                                  .ThenBy(it => it.cl.Id)
                                  .ThenBy(it => it.pa.Auxiliar != null ? it.pa.Auxiliar : it.pa.LicenseNumber)
                                  .ThenBy(it => it.ct.Name)
                                  .ToListAsync();

        TvClient lastClient = null;
        TvContractor lastContractor = null;
        TvServiceLog lastServiceLog = null;

        var clientList = new List<TvClient>();
        foreach (var it in queryRes)
        {
            if (it.cl != null && it.ct != null && it.sl != null)
            {
                var paNum = it.pa.Auxiliar != null ? it.pa.Auxiliar : it.pa.LicenseNumber; //it.pa != null ? (sufixList.Contains(it.sp.Name.Substring(3) + ";") ? it.pa.Auxiliar : it.pa.LicenseNumber) : it.cl.AuthorizationNUmber;
                if (it.cl.Id.ToString() + $"_{paNum}" != lastClient?.Id)
                {
                    clientList.Add(lastClient = new TvClient()
                    {
                        Id = it.cl.Id.ToString() + $"_{paNum}",
                        Name = it.cl.Name.Trim() + $" ({paNum})",
                    });
                    lastContractor = null; lastServiceLog = null;
                }
                if (lastContractor == null || int.Parse(lastContractor.Id) != it.ct.Id)
                {
                    lastClient.Contractors.Add(lastContractor = new TvContractor()
                    {
                        Id = it.ct.Id.ToString(),
                        Name = it.ct.Name.Trim(),
                        ContratorType = it.ctt.Name,
                        Client = lastClient
                    });
                    lastServiceLog = null;
                }

                if (lastServiceLog == null || int.Parse(lastServiceLog.Id) != it.sl.Id)
                    lastContractor.ServiceLogs.Add(lastServiceLog = new TvServiceLog()
                    {
                        Id = it.sl.Id.ToString(),
                        CreatedDate = it.sl.CreatedDate,
                        Status = (it.sl.BilledDate != null) ? "billed" : "empty",
                        Contractor = lastContractor
                    });
            }
        }
        _context.ChangeTracker.LazyLoadingEnabled = true;
        return clientList;
    }

    public async Task<Agreement> GetAgreementAsync(string companyCode, int periodID, int contractorID, int clientID)
    {
        _context.ChangeTracker.LazyLoadingEnabled = false;
        try
        {
            var infoQuery = (from ag in _context.Agreements
                             where ag.Payroll.Contractor.ServiceLogs.Any(x => x.PeriodId == periodID) &&
                                    ag.Company.Acronym == companyCode &&
                                    ag.Payroll.ContractorId == contractorID &&
                                    ag.ClientId == clientID
                             select ag)
                    .Include(y => y.Company)
                    .Include(y => y.Client.Diagnosis)
                    .Include(y => y.Payroll.Procedure)
                    .Include(y => y.Payroll.Contractor)
                    .Include(y => y.Payroll.ContractorType).Take(1);

            return await infoQuery.SingleOrDefaultAsync();
        }
        finally { _context.ChangeTracker.LazyLoadingEnabled = true; }
    }

    public async Task<List<ExtendedUnitDetail>> GetExUnitDetailsAsync(int periodID, int contractorID, int clientID, string pAccount, string sufixList)
    {
        _context.ChangeTracker.LazyLoadingEnabled = false;
        try
        {
            var infoUnitDet = from ud in _context.UnitDetails
                              join slo in _context.ServiceLogs on ud.ServiceLogId equals slo.Id
                              join sp in _context.SubProcedures on ud.SubProcedureId equals sp.Id
                              join ps in _context.PlaceOfServices on ud.PlaceOfServiceId equals ps.Id
                              join pa in _context.PatientAccounts on slo.ClientId equals pa.ClientId
                              where pa.CreateDate <= ud.DateOfService && pa.ExpireDate >= ud.DateOfService
                                 && (pAccount == pa.Auxiliar ? sufixList.Contains(sp.Name.Substring(3) + ";") : false
                                  || pAccount == pa.LicenseNumber ? !sufixList.Contains(sp.Name.Substring(3) + ";") : false)
                                 && (from sl in _context.ServiceLogs
                                     where sl.ClientId == clientID
                                        && sl.ContractorId == contractorID
                                        && sl.PeriodId == periodID
                                        && sl.Id == ud.ServiceLogId
                                     select 1).Any()
                              orderby new { ud.DateOfService, ud.SubProcedureId }
                              select new ExtendedUnitDetail() { unitDetail = ud, serviceLog = slo, subProcedure = sp, placeOfService = ps, patientAccount = pa };

            return await infoUnitDet.ToListAsync();
        }
        finally { _context.ChangeTracker.LazyLoadingEnabled = true; }
    }

    public async Task<List<ExtendedUnitDetail>> GetExUnitDetailsAsync(int serviceLogId, string pAccount, string sufixList)
    {
        _context.ChangeTracker.LazyLoadingEnabled = false;
        try
        {
            var infoUnitDet = from ud in _context.UnitDetails
                              join slo in _context.ServiceLogs on ud.ServiceLogId equals slo.Id
                              join sp in _context.SubProcedures on ud.SubProcedureId equals sp.Id
                              join ps in _context.PlaceOfServices on ud.PlaceOfServiceId equals ps.Id
                              join pa in _context.PatientAccounts on slo.ClientId equals pa.ClientId
                              where pa.CreateDate <= ud.DateOfService && pa.ExpireDate >= ud.DateOfService
                                 && (pAccount == pa.Auxiliar ? sufixList.Contains(sp.Name.Substring(3) + ";") : false
                                  || pAccount == pa.LicenseNumber ? !sufixList.Contains(sp.Name.Substring(3) + ";") : false)
                                 && slo.Id == serviceLogId
                              orderby ud.DateOfService
                              select new ExtendedUnitDetail() { unitDetail = ud, serviceLog = slo, subProcedure = sp, placeOfService = ps, patientAccount = pa };

            return await infoUnitDet.ToListAsync();
        }
        finally { _context.ChangeTracker.LazyLoadingEnabled = true; }
    }

    public async Task<ExtendedServiceLog> GetExServiceLogAsync(string companyCode, int serviceLogId)
    {
        _context.ChangeTracker.LazyLoadingEnabled = false;
        try
        {
            var infoQuery = (from sl in _context.ServiceLogs
                             join ag in _context.Agreements on new { sl.ClientId, sl.ContractorId, companyCode } equals new { ag.ClientId, ag.Payroll.ContractorId, companyCode = ag.Payroll.Company.Acronym }
                             join cl in _context.Clients on ag.ClientId equals cl.Id
                             join d in _context.Diagnoses on cl.DiagnosisId equals d.Id
                             join p in _context.Periods on sl.PeriodId equals p.Id
                             join pr in _context.Payrolls on ag.PayrollId equals pr.Id
                             join pd in _context.Procedures on pr.ProcedureId equals pd.Id
                             join ct in _context.Contractors on pr.ContractorId equals ct.Id
                             join ctt in _context.ContractorTypes on pr.ContractorTypeId equals ctt.Id
                             where sl.Id == serviceLogId
                             select new ExtendedServiceLog { serviceLog = sl, agreement = ag, client = cl, diagnosis = d, period = p, payroll = pr, procedure = pd, contractor = ct, contractorType = ctt }).Take(1);

            return await infoQuery.SingleOrDefaultAsync();
        }
        finally { _context.ChangeTracker.LazyLoadingEnabled = true; }
    }

    public async Task<object?> SetServiceLogBilled(int serviceLogId, string userId)
    {
        var servLog = await _context.ServiceLogs.SingleOrDefaultAsync(x => x.Id == serviceLogId);

        if (servLog == null)
        {
            servLog.BilledDate = DateTime.Now;
            servLog.Biller = userId;
            servLog.Pending = null;
            _context.SaveChanges();
            return new object { };
        }
        else return null;
    }

    public async Task<object?> SetServiceLogBilled(int periodId, int contratorId, int clientId, string userId)
    {
        var servLog = await _context.ServiceLogs.SingleOrDefaultAsync(x => (x.PeriodId == periodId && x.ContractorId == contratorId && x.ClientId == clientId));

        if (servLog == null)
        {
            servLog.BilledDate = DateTime.Now;
            servLog.Biller = userId;
            servLog.Pending = null;
            _context.SaveChanges();

            return new object { };
        }
        else return null;
    }

    public async Task<object?> SetServiceLogPendingReason(int serviceLogId, string reason)
    {
        var servLog = await _context.ServiceLogs.SingleOrDefaultAsync(x => x.Id == serviceLogId);

        if (servLog == null)
        {
            servLog.Pending = reason;
            _context.SaveChanges();
            return new object { };
        }
        else return null;
    }

    public async Task<IEnumerable<ManagerBiller>> GetServiceLogsBilled(int period, int company)
    {
        var query = await(from sl in _context.ServiceLogs
                          join cl in _context.Clients on sl.ClientId equals cl.Id
                          join co in _context.Contractors on sl.ContractorId equals co.Id
                          join us in _context.Users on sl.Biller equals us.Id
                          where sl.PeriodId == period && cl.Agreements.Any(ag => ag.CompanyId == company) && sl.Biller != null
                          select new ManagerBiller
                          {
                              Id = sl.Id,
                              BilledDate = (DateTime)sl.BilledDate,
                              Biller = us.UserName,
                              ClientName = cl.Name,
                              ContractorName = co.Name,
                              CreationDate = (DateTime)sl.CreatedDate
                          }).OrderBy(x => x.ClientName).ToListAsync();
        return query;
    }

    public async Task<object?> UpdateBilling(int servicelog)
    {
        var service = await _context.ServiceLogs.FirstOrDefaultAsync(sl => sl.Id == servicelog);
        if (service == null) return null;
        else
        {
            service.Biller = null;
            service.BilledDate = null;

            await _context.SaveChangesAsync();
            return new object { };
        }
    }
}
