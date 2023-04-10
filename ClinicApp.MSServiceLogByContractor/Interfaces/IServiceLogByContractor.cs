using ClinicApp.Core.Data;
using ClinicApp.Core.Models;
using ClinicApp.MSServiceLogByContractor.Dtos;

namespace ClinicApp.MSServiceLogByContractor.Interfaces
{
    public interface IServiceLogByContractor
    {
        public Task<GetContractorServiceLogDto> GetByIdAsync(int ServiceLogId);
        public Task<PagedResponse<IEnumerable<AllServiceLogDto>>> GetAllAsync(PaginationFilter filter, string route);
        public Task<GetContractorServiceLogDto> CreateAsync(CreateServiceLogDto sl);
        public Task<GetContractorServiceLogDto> UpdateAsync(int ServiceLogId, UpdateServiceLogDto sl);
        public Task<int> DeleteAsync(int ServiceLogId);

    }
}
