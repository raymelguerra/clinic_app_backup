using ClinicApp.Core.Data;
using ClinicApp.Core.Models;
using ClinicApp.MSServiceLogByContractor.Dtos;

namespace ClinicApp.MSServiceLogByContractor.Interfaces
{
    public interface IServiceLogByContractor
    {
        public Task<GetContractorServiceLogDto> GetByIdAsync(int ServiceLogId);
        public Task<IEnumerable<AllServiceLogDto>> GetAllAsync(int ContractorId);
        public Task<GetContractorServiceLogDto> CreateAsync(CreateServiceLogDto sl);
        public Task<GetContractorServiceLogDto> UpdateAsync(int ServiceLogId, UpdateServiceLogDto sl);
        public Task<int> CreateUserContractorAsync(CreateUserContractor user);
        public Task<int> DeleteAsync(int ServiceLogId);

    }
}
