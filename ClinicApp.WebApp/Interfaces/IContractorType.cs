using ClinicApp.Core.Models;

namespace ClinicApp.WebApp.Interfaces
{
    public interface IContractorType
    {
        public Task<IEnumerable<ContractorType>> GetContractorTypeAsync(string filter);
        public Task<ContractorType?> GetContractorTypeAsync(int id);
    }
}
