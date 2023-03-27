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
        public async Task<GetContractorServiceLogDto> CreateAsync(CreateServiceLogDto sl)
        {
            // Create service logs and dependencies
            var serv = new ServiceLog
            {
                PeriodId = sl.PeriodId,
                ContractorId = sl.ContractorId,
                ClientId = sl.ClientId,
                CreatedDate = sl.CreatedDate,
                Pending = sl.Pending,
                Status = sl.Status
            };

            _db.ServiceLogs.Add(serv);
            await _db.SaveChangesAsync();

            var ctrServ = new ContractorServiceLog
            {
                ServiceLogId = serv.Id,
                Signature = sl.Signature,
                SignatureDate = sl.SignatureDate
            };
            _db.ContractorServiceLog.Add(ctrServ);
            await _db.SaveChangesAsync();

            // create Unit Details and dependencies
            foreach (var item in sl.UnitDetails)
            {
                var ud = new UnitDetail
                {
                    DateOfService = item.DateOfService,
                    Modifiers = item.Modifiers,
                    PlaceOfServiceId = item.PlaceOfServiceId,
                    ServiceLogId = serv.Id,
                    SubProcedureId = item.SubProcedureId,
                    Unit = item.Unit
                };

                _db.UnitDetails.Add(ud);
                await _db.SaveChangesAsync();
                
                var ptUnit = new PatientUnitDetail
                {
                    DepartureTime = item.DepartureTime,
                    EntryTime = item.EntryTime,
                    UnitDetailId = ud.Id,
                    Signature = item.PatientSignature,
                    SignatureDate = item.PatientSignatureDate
                };
                _db.PatientUnitDetail.Add(ptUnit);
                await _db.SaveChangesAsync();
            }

            return await GetByIdAsync(serv.Id);
        }

        public async Task<object?> DeleteAsync(int ServiceLogId)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResponse<IEnumerable<AllServiceLogDto>>> GetAllAsync(PaginationFilter filter, string route, int ContractorId)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var list = await _db.ServiceLogs.Include("Client").Include("Period")
                .Where( ctr => ctr.ContractorId == ContractorId)
                .Select(x => new AllServiceLogDto
                {
                    ServiceLogId = x.Id,
                    ClientName = x.Client.Name!,
                    CreatedDate = (DateTime)x.CreatedDate!,
                    PeriodRange = $"{x.Period.StartDate} - {x.Period.EndDate}",
                    ServiceLogStatus = (ServiceLogStatus)x.Status
                })
                // .OrderByDescending(x => x.CreatedDate)
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
                    ServiceLogId = sl.ServiceLogId,
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

        public async Task<object?> UpdateAsync(int ServiceLogId, CreateServiceLogDto sl)
        {
            throw new NotImplementedException();
        }
    }
}
