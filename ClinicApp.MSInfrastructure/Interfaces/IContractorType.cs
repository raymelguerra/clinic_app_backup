using ClinicApp.Core.Models;

namespace ClinicApp.MSInfrastructure.Interfaces
{
    public interface IContractorType
    {
        public Task<IEnumerable<ContractorType>> Get();
        public Task<ContractorType?> Get(int id);
    }
}
