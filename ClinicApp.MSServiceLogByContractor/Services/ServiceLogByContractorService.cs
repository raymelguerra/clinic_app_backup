using ClinicApp.Core.Data;
using ClinicApp.Core.Interfaces;
using ClinicApp.Core.Models;
using ClinicApp.MSServiceLogByContractor.Dtos;
using ClinicApp.MSServiceLogByContractor.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Runtime.ConstrainedExecution;
using System;
using Microsoft.AspNetCore.Components.Forms;

namespace ClinicApp.MSServiceLogByContractor.Services;

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
        DateTime now = sl.CreatedDate ?? DateTime.Now;
        // Create service logs and dependencies
        var serv = new ServiceLog
        {
            PeriodId = sl.PeriodId,
            ContractorId = sl.ContractorId,
            ClientId = sl.ClientId,
            CreatedDate = now,
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
            //bool validUnit = IsValidUnit(sl.ClientId, sl.ContractorId, item.Unit);
            //if (!validUnit)
            //{
            //    throw new ArgumentException("El valor del campo Unit no cumple con las condiciones de validación");
            //}

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
                Signature = item.Signature,
                SignatureDate = item.SignatureDate
            };
            _db.PatientUnitDetail.Add(ptUnit);
            await _db.SaveChangesAsync();
        }

        return await GetByIdAsync(serv.Id);
    }


    public bool IsValidUnit(int clientId, int contractorId, int unit)
    {
        int weeklyApprovedValue = 0;
        int contractorTypeId = _db.Payrolls
            .Where(p => p.ContractorId == contractorId)
            .Select(p => p.ContractorTypeId)
            .FirstOrDefault();

        if (contractorTypeId == 1)
        {
            weeklyApprovedValue = _db.Clients
                .Where(c => c.Id == clientId)
                .Select(c => c.WeeklyApprovedAnalyst)
                .FirstOrDefault();
        }
        else
        {
            weeklyApprovedValue = _db.Clients
                .Where(c => c.Id == clientId)
                .Select(c => c.WeeklyApprovedRbt)
                .FirstOrDefault();
        }
        int unitInMinutes = unit * 15;
        bool isUnitLessThanWeeklyApproved = unitInMinutes < weeklyApprovedValue;
        return isUnitLessThanWeeklyApproved;
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

    public async Task<IEnumerable<AllServiceLogDto>> GetAllAsync(int ContractorId)
    {
        var list = await _db.ServiceLogs.Include("Client").Include("Period")
            .Where(ctr => ctr.ContractorId == ContractorId)
            .Select(x => new AllServiceLogDto
            {
                ServiceLogId = x.Id,
                ClientName = x.Client.Name!,
                CreatedDate = (DateTime)x.CreatedDate!,
                PeriodRange = $"{x.Period.StartDate.ToString("MM/dd/yyyy")} - {x.Period.EndDate.ToString("MM/dd/yyyy")}",
                ServiceLogStatus = (ServiceLogStatus)x.Status
            }).ToListAsync();
        var totalRecords = await _db.ServiceLogs.CountAsync();

        // var pagedReponse = PaginationHelper.CreatePagedReponse<AllServiceLogDto>(list, validFilter, totalRecords, _uriService, route);
        return list;
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

        // DateTime now = sl.CreatedDate ?? DateTime.Now;
        serviceLog.PeriodId = sl.PeriodId;
        serviceLog.ContractorId = sl.ContractorId;
        serviceLog.ClientId = sl.ClientId;
        // serviceLog.CreatedDate = now;
        //  serviceLog.Pending = sl.Pending;
        // serviceLog.Status = sl.Status;

        var contractorServiceLog = await _db.ContractorServiceLog.FirstOrDefaultAsync(c => c.ServiceLogId == serviceLog.Id);

        if (contractorServiceLog == null)
        {
            throw new ArgumentException("ContractorServicelog is null and the operation cannot be performed.");
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



        foreach (var item in sl.UnitDetails)
        {
            var ud = new UnitDetail
            {
                DateOfService = item.DateOfService,
                PlaceOfServiceId = Convert.ToInt32(item.PlaceOfServiceId),
                ServiceLogId = serviceLog.Id,
                SubProcedureId = Convert.ToInt32(item.SubProcedureId),
            };

            _db.UnitDetails.Add(ud);
            await _db.SaveChangesAsync();
            var ptUnit = new PatientUnitDetail
            {
                DepartureTime = item.DepartureTime,
                EntryTime = item.EntryTime,
                UnitDetailId = ud.Id,
                Signature = item.Signature,
                SignatureDate = item.SignatureDate,
            };

            _db.PatientUnitDetail.Add(ptUnit);
        }

        await _db.SaveChangesAsync();

        return await GetByIdAsync(contractorServiceLog.ServiceLogId);
    }

    public async Task<int> CreateUserContractorAsync(CreateUserContractor user)
    {
        bool _ctr = false, _user = false;
        if (await _db.Contractors.AnyAsync(x => x.Id == user.ContractorId))
            _ctr = true;
        if (await _db.Users.AnyAsync(x => x.Id == user.UserId))
            _user = true;
        if (_ctr && _user)
        {
            _db.ContractorUser.Add(new ContractorUser
            {
                ContractorId = user.ContractorId,
                UserId = user.UserId!
            });
            await _db.SaveChangesAsync();
            return 1;
        }
        return 0;

    }

    public async Task<CreateValidationDto> PreCreateValidateAsync(CreateServiceLogDto sl)
    {
        /*
         Requirement 
          Validaciones por cliente
            1. Todos los contractors 53 55HN 55 56 con un mismo cliente. La suma de todas las horas de estos codigos el mismo  dia no puede ser mayor a 8 (32 unidades). (Las horas de los XP no suman)
            2. El total de horas en una semana para un cliente no puede exceder las 40 horas (160 unidades) (excepto para los XP).

          Validaciones de los contractors
            1. El total de horas trabajadas en el dia (con todos los clientes) no puede exceder las 10 horas (40 unidades).
            2. Para el caso de los 53XP cuando vayan a insertar una linea de servicio solo se puede insertar dentro de un rango horario realizado por los codigos 55 y 55HN.
         */

        CreateValidationDto resp = new();
        var message = new List<string>();
        var first = await this.CountHoursDailybyClientAsync(sl.CreatedDate!.Value, sl.ClientId, sl.UnitDetails);
        if (first != String.Empty)
            message.Add(first);
        var second = await this.CountHoursWeeklybyClientAsync(sl.CreatedDate!.Value, sl.ClientId, sl.UnitDetails);
        if (second != String.Empty)
            message.Add(second);

        var third = await this.CountHoursDailybyContractorAsync(sl.CreatedDate!.Value, sl.ContractorId, sl.UnitDetails);
        if (third != String.Empty)
            message.Add(third);

        var fourth = await this.VerifyXPContractorAsync(sl.CreatedDate!.Value, sl.ClientId, sl.UnitDetails);
        if (fourth != String.Empty)
            message.Add(fourth);

        resp.Message = message;

        return resp;
    }
    private async Task<string> CountHoursDailybyClientAsync(DateTime createDate, int clientId, IEnumerable<CreateUnitDetail> units = null!)
    {
        if (units != null)
        {
            var servLogs = await _db.ServiceLogs.Include(x => x.UnitDetails).Where(x => x.ClientId == clientId && DateTime.Compare(x.CreatedDate!.Value.Date, createDate.Date) == 0).ToListAsync<ServiceLog>();

            var subproc = await _db.SubProcedures.Where(x => x.Name.Contains("XP")).Select(x => x.Id).ToListAsync();
            int sumatory = 0;
            foreach (var item in servLogs)
            {
                sumatory += item.UnitDetails.Where(x => !subproc.Contains(x.SubProcedureId)).Select(x => x.Unit).Sum();
            }

            int sum = units.Where(x => !subproc.Contains(x.SubProcedureId)).Select(x => x.Unit).Sum();

            return (sum + sumatory > 32) ? "The number of daily hours with a client cannot exceed 32 units" : String.Empty;
        }
        return String.Empty;
    }

    private async Task<string> CountHoursWeeklybyClientAsync(DateTime createDate, int clientId, IEnumerable<CreateUnitDetail> units = null!)
    {
        if (units != null)
        {
            var servLogs = await _db.ServiceLogs.Include(x => x.UnitDetails).Where(x => x.ClientId == clientId && x.CreatedDate!.Value.DayOfWeek == createDate.DayOfWeek).ToListAsync<ServiceLog>();

            var subproc = await _db.SubProcedures.Where(x => x.Name.Contains("XP")).Select(x => x.Id).ToListAsync();
            int sumatory = 0;
            foreach (var item in servLogs)
            {
                sumatory += item.UnitDetails.Where(x => !subproc.Contains(x.SubProcedureId)).Select(x => x.Unit).Sum();
            }

            int sum = units.Where(x => !subproc.Contains(x.SubProcedureId)).Select(x => x.Unit).Sum();

            return (sum + sumatory > 160) ? "The number of weekly hours with a client cannot exceed 160 units" : String.Empty;
        }
        return String.Empty;
    }

    private async Task<string> CountHoursDailybyContractorAsync(DateTime createDate, int contractorId, IEnumerable<CreateUnitDetail> units = null!)
    {
        if (units != null)
        {
            var servLogs = await _db.ServiceLogs.Include(x => x.UnitDetails)
                .Where(x => x.ContractorId == contractorId && DateTime.Compare(x.CreatedDate!.Value.Date, createDate.Date) == 0)
                .ToListAsync<ServiceLog>();

            var subproc = await _db.SubProcedures
                .Where(x => x.Name.Contains("XP"))
                .Select(x => x.Id)
                .ToListAsync();

            int sumatory = 0;
            foreach (var item in servLogs)
            {
                sumatory += item.UnitDetails
                    .Where(x => !subproc.Contains(x.SubProcedureId))
                    .Select(x => x.Unit)
                    .Sum();
            }

            int sum = units
                .Where(x => !subproc.Contains(x.SubProcedureId))
                .Select(x => x.Unit)
                .Sum();

            return (sum + sumatory > 40) ? "The number of daily hours for a contractor cannot exceed 40 units" : String.Empty;
        }
        return String.Empty;
    }

    private async Task<string> VerifyXPContractorAsync(DateTime createDate, int clientId, IEnumerable<CreateUnitDetail> units = null!)
    {
        if (units != null)
        {
            var servLogs = await _db.ServiceLogs.Include(x => x.UnitDetails).Where(x => x.ClientId == clientId && DateTime.Compare(x.CreatedDate!.Value.Date, createDate.Date) == 0).ToListAsync<ServiceLog>();

            if (servLogs.Count() == 0) return "No analysts found at the specified XP code(s) time";

            var subProcedures = await _db.SubProcedures.ToListAsync();
            var subProcXp = subProcedures.Where(x => x.Name.Contains("53XP")).Select(x => x.Id).ToList();
            var subProcAnalyst = subProcedures.Where(x => x.Name.Contains("55") || x.Name.Contains("55HN")).Select(x => x.Id).ToList();

            bool checker = false;
            foreach (var item in units.Where(x => subProcXp.Contains(x.SubProcedureId)))
            {
                foreach (var serv in servLogs)
                {
                    var unitsCtr = serv.UnitDetails.Where(x => !subProcAnalyst.Contains(x.SubProcedureId)).Select(x => x.Id).ToList();
                    if (unitsCtr.Count() > 0)
                    {                        
                        checker = await _db.PatientUnitDetail
                            .Where(
                                x => unitsCtr.Contains(x.UnitDetailId)
                                 &&
                                this.IsInRange(
                                    item.EntryTime!,
                                    item.DepartureTime!,
                                    x.EntryTime,
                                    x.DepartureTime)
                                )
                            .CountAsync() > 0;

                    }
                }
            }
            return !checker ? "No analysts found at the specified XP code(s) time" : string.Empty;
        }
        return String.Empty;
    }
    private bool IsInRange(string startNew, string endNew, string startCreated="", string endCreated = "")
    {
        return (TimeSpan.Parse(startNew) >= TimeSpan.Parse(startCreated)) && (TimeSpan.Parse(endNew) <= TimeSpan.Parse(endCreated));
    }
}