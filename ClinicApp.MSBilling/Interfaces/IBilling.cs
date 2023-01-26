using ClinicApp.Core.DTO;
using ClinicApp.Core.Models;
using ClinicApp.MSBilling.Dtos;
using Microsoft.EntityFrameworkCore;

namespace ClinicApp.MSBilling.Interfaces
{
    public interface IBilling
    {
        public Task<List<ExtendedPeriod>> GetPeriodsAsync();
        public Task<Period> GetPeriodAsync(int periodID);
        public Task<List<Company>> GetCompaniesAsync();
        public Task<List<TvFullData>> GetContractorAndClientsAsync(string CompanyCode, int PeriodId);
        public Task<Agreement> GetAgreementAsync(string companyCode, int periodID, int contractorID, int clientID);
        public Task<List<UnitDetailDto>> GetExUnitDetailsAsync(int periodID, int contractorID, int clientID, string pAccount, string sufixList);
        public Task<List<UnitDetailDto>> GetExUnitDetailsAsync(int serviceLogId, string pAccount, string sufixList);
        public Task<ExtendedServiceLog> GetExServiceLogAsync(string companyCode, int serviceLogId);
        public Task<object?> SetServiceLogBilled(int serviceLogId, string userId);
        public Task<object?> SetServiceLogBilled(int periodId, int contratorId, int clientId, string userId);
        public Task<object?> SetServiceLogPendingReason(int serviceLogId, string reason);
        public Task<IEnumerable<ManagerBiller>> GetServiceLogsBilled(int period, int company);
        public Task<object?> UpdateBilling(int servicelog);
    }
}
