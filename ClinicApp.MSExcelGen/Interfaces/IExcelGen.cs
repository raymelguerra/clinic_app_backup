using ClinicApp.Core.DTO;
using ClinicApp.Core.Models;
using ClinicApp.MSExcelGen.Dtos;

namespace ClinicApp.MSExcelGen.Interfaces
{
    public interface IExcelGen
    {
        public Task<List<CompanyDto>> GetCompanies();

        public Task<List<ExtendedPeriod>> GetPeriodsAsync();

        public Task<ExtendedPeriod> GetPeriodAsync(int PeriodId = -1);

        public Task<List<ExtendedContractor>> GetExContractorsAsync(string companyCode);

        public Task<List<AgreementDto>> GetAgreementsAsync(int ContractorId, int CompanyId);

        public Task<List<ExtendedAgrUnitDetail>> GetExAgrUnitDetails(int ClientId, int ContractorId, int CompanyId, int PeriodId);
    }
}
