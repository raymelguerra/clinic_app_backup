using ClinicApp.Core.DTO;
using ClinicApp.Core.Models;

namespace ClinicApp.MSExcelGen.Interfaces
{
    public interface IExcelGen
    {


        public Task<List<Company>> GetCompanies();

        public Task<List<ExtendedPeriod>> GetPeriodsAsync();

        public Task<Period> GetPeriodAsync(int PeriodId = -1);

        public Task<List<ExtendedContractor>> GetExContractorsAsync(string companyCode);

        public Task<List<Agreement>> GetAgreementsAsync(int ContractorId, int CompanyId);

        public Task<List<ExtendedAgrUnitDetail>> GetExAgrUnitDetails(int ClientId, int ContractorId, int CompanyId, int PeriodId);
    }
}
