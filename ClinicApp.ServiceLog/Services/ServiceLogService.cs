using ClinicApp.MSServiceLog.Interfaces;
using ClinicApp.Core.Data;
using ClinicApp.Core.Interfaces;
using ClinicApp.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClinicApp.MSServiceLog.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using ClinicApp.MSServiceLog.Dtos;

namespace ClinicApp.MSServiceLog.Services;

public class ServiceLogService : IServiceLog
{
    private readonly ClinicbdMigrationContext _context;
    private readonly IUriService _uriService;
    public ServiceLogService(ClinicbdMigrationContext context, IUriService uriService)
    {
        _context = context;
        _uriService = uriService;
    }

    public async Task<object?> DeleteServiceLog(int id)
    {
        var serviceLog = await _context.ServiceLogs.FindAsync(id);
        if (serviceLog == null)
        {
            return null;
        }

        var unitDetails = await _context.UnitDetails.Where(x => x.ServiceLogId == serviceLog.Id).ToListAsync();
        _context.UnitDetails.RemoveRange(unitDetails);
        await _context.SaveChangesAsync();

        _context.ServiceLogs.Remove(serviceLog);
        await _context.SaveChangesAsync();

        return new ServiceLog { };
    }

    public async Task<PagedResponse<IEnumerable<ServiceLog?>>> GetPendingServiceLog(PaginationFilter filter, string route)
    {
        var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
        var list = await _context.ServiceLogs.Include("Client").Include("Contractor").Include("Period")
            .Where(pen => pen.Pending != null)
            .OrderByDescending(x => x.Period.PayPeriod)
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToListAsync();
        var totalRecords = await _context.ServiceLogs.Where(pen => pen.Pending != null).CountAsync();

        var pagedReponse = PaginationHelper.CreatePagedReponse<ServiceLog>(list, validFilter, totalRecords, _uriService, route);
        return pagedReponse!;
    }

    public async Task<PagedResponse<IEnumerable<ServiceLog>>> GetServiceLog(PaginationFilter filter, string route)
    {
        var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
        var list = await _context.ServiceLogs.Include("Client").Include("Contractor").Include("Period")
            .OrderByDescending(x => x.CreatedDate)
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToListAsync();
        var totalRecords = await _context.ServiceLogs.CountAsync();

        var pagedReponse = PaginationHelper.CreatePagedReponse<ServiceLog>(list, validFilter, totalRecords, _uriService, route);
        return pagedReponse;
    }


    public async Task<ServiceLog?> GetServiceLog(int id)
    {
        var serviceLog = await _context.ServiceLogs.FindAsync(id);

        if (serviceLog == null)
        {
            return null;
        }

        return await _context.ServiceLogs.Include("Contractor").Include("Client").Include("Period").Select(x => new ServiceLog
        {
            Id = x.Id,
            Period = x.Period,
            PeriodId = x.PeriodId,
            Contractor = x.Contractor,
            ContractorId = x.ContractorId,
            Client = x.Client,
            ClientId = x.ClientId,
            UnitDetails = _context.UnitDetails.Include("SubProcedure").Include("PlaceOfService").Where(ud => ud.ServiceLogId == x.Id).ToList(),
        }).Where(x => x.Id == serviceLog.Id).FirstOrDefaultAsync();
    }

    public async Task<PagedResponse<IEnumerable<ServiceLogByNameDto>>> GetServiceLogByName(PaginationFilter filter, string name, string route, string type)
    {
        var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
        if (type.Equals("Client"))
        {
            var list = await _context.ServiceLogs
                .Include("Client")
                .Include("Contractor")
                .Include("Period")
                .Select(x => new ServiceLogByNameDto
                {
                    BilledDate = x.BilledDate,
                    Client = new ClientDto
                    {
                        Id = x.Client.Id,
                        Name = x.Client.Name
                    },
                    Id = x.Id,
                    Contractor = new ContractorDto
                    {
                        Id = x.Contractor.Id,
                        Name = x.Contractor.Name
                    },
                    Biller = x.Biller,
                    ClientId = x.ClientId,
                    ContractorId = x.Contractor.Id,
                    CreatedDate = x.CreatedDate,
                    Pending = x.Pending,
                    Period = new (){
                        Active = x.Period.Active,
                        DocumentDeliveryDate = x.Period.DocumentDeliveryDate,
                        EndDate = x.Period.EndDate,
                        Id = x.Period.Id,
                        PaymentDate = x.Period.PaymentDate,
                        PayPeriod = x.Period.PayPeriod,
                        StartDate = x.Period.StartDate
                    }
                })
                .Where(x => (x.Client.Name!.ToUpper().Contains(name.ToUpper())))
                .OrderByDescending(x => x.CreatedDate)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();

            var totalRecords = await _context.ServiceLogs.Include("Client")
                .Where(x => (x.Client.Name!.ToUpper().Contains(name.ToUpper()))).CountAsync();

            var pagedReponse = PaginationHelper.CreatePagedReponse<ServiceLogByNameDto>(list, validFilter, totalRecords, _uriService, route);
            return pagedReponse;
        }
        else
        {
            var list = await _context.ServiceLogs.Include("Client").Include("Contractor").Include("Period")
                    .Where(x => (x.Contractor.Name!.ToUpper().Contains(name.ToUpper())))
                    .Select(x => new ServiceLogByNameDto
                    {
                        BilledDate = x.BilledDate,
                        Client = new ClientDto
                        {
                            Id = x.Client.Id,
                            Name = x.Client.Name
                        },
                        Id = x.Id,
                        Contractor = new ContractorDto
                        {
                            Id = x.Contractor.Id,
                            Name = x.Contractor.Name
                        },
                        Biller = x.Biller,
                        ClientId = x.ClientId,
                        ContractorId = x.Contractor.Id,
                        CreatedDate = x.CreatedDate,
                        Pending = x.Pending,
                        Period = new()
                        {
                            Active = x.Period.Active,
                            DocumentDeliveryDate = x.Period.DocumentDeliveryDate,
                            EndDate = x.Period.EndDate,
                            Id = x.Period.Id,
                            PaymentDate = x.Period.PaymentDate,
                            PayPeriod = x.Period.PayPeriod,
                            StartDate = x.Period.StartDate
                        }
                    })
                    .OrderByDescending(x => x.CreatedDate)
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync<ServiceLogByNameDto>();

            var totalRecords = await _context.ServiceLogs.Include("Contractor")
                .Where(x => x.Contractor.Name!.ToUpper().Contains(name.ToUpper())).CountAsync();

            var pagedReponse = PaginationHelper.CreatePagedReponse<ServiceLogByNameDto>(list, validFilter, totalRecords, _uriService, route);
            return pagedReponse;
        }
    }

    public async Task<IEnumerable<ServiceLogWithoutDetailsDto>> GetServiceLogWithoutDetails()
    {
        var list = await (from sl in _context.ServiceLogs
                          select new ServiceLogWithoutDetailsDto
                          {
                              ClientName = sl.Client.Name,
                              ContractorName = sl.Contractor.Name,
                              StartDate = sl.Period.StartDate,
                              EndDate = sl.Period.EndDate,
                              OrderBY = sl.CreatedDate
                          }
                               ).OrderByDescending(x => x.OrderBY).ToListAsync();
        return list;
    }

    public async Task<ServiceLog?> PostServiceLog(ServiceLog serviceLog)
    {
        _context.ServiceLogs.Add(serviceLog);

        await _context.SaveChangesAsync();

        return await _context.ServiceLogs.Include("Contractor").Include("Client").Include("Period").Where(x => x.Id == serviceLog.Id).FirstOrDefaultAsync();
    }

    public async Task<object?> PutServiceLog(int id, ServiceLog serviceLog, bool partial = true)
    {

        var oldServiceLog = await _context.ServiceLogs.Include("UnitDetails").Where(x => x.Id == id).FirstOrDefaultAsync();
        // Add olds
        foreach (var olds in oldServiceLog!.UnitDetails.ToList())
        {
            var found = false;

            foreach (var news in serviceLog.UnitDetails.ToList())
            {
                if (olds.Id == news.Id)
                {
                    // Update if changed
                    if (olds.PlaceOfServiceId != news.PlaceOfServiceId || olds.Unit != news.Unit || olds.DateOfService != news.DateOfService)
                    {
                        olds.DateOfService = news.DateOfService;
                        olds.PlaceOfServiceId = news.PlaceOfServiceId;
                        olds.Unit = news.Unit;
                    }
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                oldServiceLog.UnitDetails.Remove(olds);
            }
        }

        // Add news
        var newsUD = serviceLog.UnitDetails.Where(x => x.Id == 0).ToList();
        foreach (var item in newsUD)
        {
            oldServiceLog.UnitDetails.Add(item);
        }

        // update self field
        oldServiceLog.PeriodId = serviceLog.PeriodId;
        oldServiceLog.ContractorId = serviceLog.ContractorId;
        oldServiceLog.ClientId = serviceLog.ClientId;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ServiceLogExists(id))
            {
                return null;
            }
            else
            {
                throw;
            }
        }

        return new ServiceLog { };
    }

    public bool ServiceLogExists(int id)
    {
        return _context.ServiceLogs.Any(e => e.Id == id);
    }

    public async Task<ServiceLog?> UpdatePendingStatus(int id)
    {
        var sl = await _context.ServiceLogs.Where(x => x.Id == id).FirstOrDefaultAsync();

        if (sl == null)
            return null;

        sl.Pending = null;

        await _context.SaveChangesAsync();

        return new ServiceLog { };
    }
    public async Task<PagedResponse<IEnumerable<ServiceLog?>>> GetServiceLogsByName(PaginationFilter filter, string name, string type, string route)
    {
        var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
        if (type.Equals("Client"))
        {
            var list = await _context.ServiceLogs
                .Include("Client")
                .Include("Contractor")
                .Include("Period")
                .Where(x => (x.Client.Name!.ToUpper().Contains(name.ToUpper())))
                .OrderBy(x => x.Period.StartDate)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();

            var totalRecords = await _context.ServiceLogs.Include("Client").Include("Contractor").Include("Period")
                .Where(x => (x.Client.Name!.ToUpper().Contains(name.ToUpper()))).CountAsync();

            var pagedReponse = PaginationHelper.CreatePagedReponse<ServiceLog>(list, validFilter, totalRecords, _uriService, route);
            return pagedReponse!;
        }
        else
        {
            var list = await _context.ServiceLogs.Include("Client").Include("Contractor").Include("Period")
                    .Where(x => (x.Contractor.Name!.ToUpper().Contains(name.ToUpper())))
                    .OrderBy(x => x.Period.StartDate)
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();

            var totalRecords = await _context.ServiceLogs.Include("Client").Include("Contractor").Include("Period")
                .Where(x => x.Contractor.Name!.ToUpper().Contains(name.ToUpper())).CountAsync();

            var pagedReponse = PaginationHelper.CreatePagedReponse<ServiceLog>(list, validFilter, totalRecords, _uriService, route);
            return pagedReponse!;
        }
    }
}
