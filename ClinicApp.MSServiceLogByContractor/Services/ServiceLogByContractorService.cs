using ClinicApp.Core.Data;
using ClinicApp.Core.Interfaces;
using ClinicApp.Core.Models;
using ClinicApp.MSServiceLogByContractor.Dtos;
using ClinicApp.MSServiceLogByContractor.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicApp.MSServiceLogByContractor.Services
{
    public class ServiceLogByContractorService : IServiceLogByContractor
    {
        private readonly ClinicbdMigrationContext _db;
        private readonly IUriService _uriService;
        public ServiceLogByContractorService(ClinicbdMigrationContext db, IUriService uriService)
        {
            _db = db;
            _uriService = uriService;
        }
        public async Task<AllServiceLogDto> CreateAsync(ServiceLog sl)
        {
            throw new NotImplementedException();
        }

        public async Task<object?> DeleteAsync(int ServiceLogId)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResponse<IEnumerable<AllServiceLogDto>>> GetAllAsync(PaginationFilter filter, string route)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var list = await _db.ServiceLogs.Include("Client").Include("Period")
                .Select(x => new AllServiceLogDto
                {
                    ServiceLogId = x.Id,
                    ClientName = x.Client.Name!,
                    CreatedDate = x.CreatedDate,
                    PeriodRange = $"{x.Period.StartDate} - {x.Period.EndDate}",
                    ServiceLogStatus = (ServiceLogStatus)x.Status
                })
                .OrderByDescending(x => x.CreatedDate)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
                .ToListAsync();
            var totalRecords = await _db.ServiceLogs.CountAsync();

            var pagedReponse = PaginationHelper.CreatePagedReponse<AllServiceLogDto>(list, validFilter, totalRecords, _uriService, route);
            return pagedReponse;
        }

        public async Task<GetContractorServiceLogDto> GetByIdAsync(int ServiceLogId)
        {
            return await _db.ContractorServiceLog.Where(x => x.ServiceLogId == ServiceLogId)
                .Select(sl => new GetContractorServiceLogDto
                {
                    Id = sl.Id,
                    Signature = sl.Signature,
                    PatientUnitDetails = _db.PatientUnitDetail
                    .Include("UnitDetail")
                    .Where(ud => ud.UnitDetail.ServiceLogId == ServiceLogId)
                    .Select(pud => new GetPatientUnitDetail
                    {
                        DepartureTime = pud.DepartureTime,
                        EntryTime = pud.EntryTime,
                        Id = pud.Id,
                        PlaceOfServiceId = pud.UnitDetail.PlaceOfServiceId,
                        ProcedureId = pud.UnitDetail.SubProcedureId,
                        Signature = pud.Signature,
                        UnitDetailId = pud.UnitDetailId
                    })
                    .ToList()
                })
                .FirstAsync();
        }

        public async Task<object?> UpdateAsync(int ServiceLogId, ServiceLog sl)
        {
            throw new NotImplementedException();
        }
    }
}
