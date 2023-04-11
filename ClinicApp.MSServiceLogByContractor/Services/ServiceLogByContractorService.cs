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
        public async Task<int> DeleteAsync(int ServiceLogId)
        {

            //// Si no se encuentra el registro de servicio, se devuelve 0
            var serviceLog = await _db.ServiceLogs.FindAsync(ServiceLogId);

            if (serviceLog == null)
            {

                return 0;
            }

            //get Unitdetail and delete PatientUnitDetail
            var unitDetails = await _db.UnitDetails
                .Where(ud => ud.ServiceLogId == ServiceLogId)
                .ToListAsync();


            foreach (var unitDetail in unitDetails)
            {
                var patientUnitDetails = await _db.PatientUnitDetail
                    .Where(ptud => ptud.UnitDetailId == unitDetail.Id)
                    .ToListAsync();

                _db.PatientUnitDetail.RemoveRange(patientUnitDetails);
                _db.UnitDetails.Remove(unitDetail);
            }

            // Delete ContractorServicelog
            var contractorServiceLog = await _db.ContractorServiceLog
                .FirstOrDefaultAsync(csl => csl.ServiceLogId == ServiceLogId);

            if (contractorServiceLog != null)
            {
                _db.ContractorServiceLog.Remove(contractorServiceLog);
            }

            _db.ServiceLogs.Remove(serviceLog);


            return await _db.SaveChangesAsync();
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
            return await _db.ContractorServiceLog.Include("ServiceLog").Where(x => x.ServiceLogId == ServiceLogId)
                .Select(sl => new GetContractorServiceLogDto
                {
                    Id = sl.Id,
                    ClientId = sl.ServiceLog.ClientId,
                    PeriodId = sl.ServiceLog.PeriodId,
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

        public async Task<GetContractorServiceLogDto> UpdateAsync(int ServiceLogId, UpdateServiceLogDto sl)
        {
            var serviceLog = await _db.ServiceLogs.FindAsync(ServiceLogId);

            if (serviceLog == null)
            {
                throw new ArgumentException("Service log no existe, no se puede realizar la operación.");
            }

            serviceLog.PeriodId = sl.PeriodId;
            serviceLog.ContractorId = sl.ContractorId;
            serviceLog.ClientId = sl.ClientId;
            serviceLog.CreatedDate = sl.CreatedDate;
            serviceLog.Pending = sl.Pending;
            serviceLog.Status = sl.Status;

            var contractorServiceLog = await _db.ContractorServiceLog.FirstOrDefaultAsync(c => c.ServiceLogId == serviceLog.Id);

            if (contractorServiceLog == null)
            {
                throw new ArgumentException("ContractorServicelog es nulo y no se puede realizar la operación.");
            }

            contractorServiceLog.Signature = sl.Signature;
            contractorServiceLog.SignatureDate = sl.SignatureDate;
            // Eliminar todas las entidades PatientUnitDetail y UnitDetail asociadas al ServiceLog
            var existingPatientUnitDetails = await _db.PatientUnitDetail.Include(p => p.UnitDetail)
                .Where(p => p.UnitDetail.ServiceLogId == serviceLog.Id).ToListAsync();
            _db.PatientUnitDetail.RemoveRange(existingPatientUnitDetails);
            await _db.SaveChangesAsync();

            var existingUnitDetails = await _db.UnitDetails.Where(u => u.ServiceLogId == serviceLog.Id).ToListAsync();
            _db.UnitDetails.RemoveRange(existingUnitDetails);

            // Agregar las nuevas entidades PatientUnitDetail y UnitDetail al ServiceLog
            foreach (var item in sl.UnitDetails)
            {
                var ud = new UnitDetail
                {
                    DateOfService = item.DateOfService,
                    Modifiers = item.Modifiers,
                    PlaceOfServiceId = item.PlaceOfServiceId,
                    ServiceLogId = serviceLog.Id,
                    SubProcedureId = item.SubProcedureId,
                    Unit = item.Unit
                };

                _db.UnitDetails.Add(ud);
                await _db.SaveChangesAsync();
                var ptUnit = new PatientUnitDetail
                { 
                    DepartureTime = item.PatientUnitDetail.DepartureTime,
                    EntryTime = item.PatientUnitDetail.EntryTime,
                    UnitDetailId = ud.Id,
                    Signature = item.PatientUnitDetail.PatientSignature,
                    SignatureDate = item.PatientUnitDetail.PatientSignatureDate
                };

                _db.PatientUnitDetail.Add(ptUnit);
            }

            await _db.SaveChangesAsync();

            return await GetByIdAsync(contractorServiceLog.ServiceLogId);
        }
    }
}
