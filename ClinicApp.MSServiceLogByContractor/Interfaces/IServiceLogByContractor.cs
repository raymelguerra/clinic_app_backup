using ClinicApp.Core.Data;
using ClinicApp.Core.Models;
using ClinicApp.MSServiceLogByContractor.Dtos;

namespace ClinicApp.MSServiceLogByContractor.Interfaces
{
    public interface IServiceLogByContractor
    {
        public Task<GetContractorServiceLogDto> GetByIdAsync(int ServiceLogId);
        public Task<PagedResponse<IEnumerable<AllServiceLogDto>>> GetAllAsync(PaginationFilter filter, string route, int ContractorId);
        public Task<GetContractorServiceLogDto> CreateAsync(CreateServiceLogDto sl);
        public Task<object?> UpdateAsync(int ServiceLogId, CreateServiceLogDto sl);
        public Task<object?> DeleteAsync(int ServiceLogId);

    }
}
