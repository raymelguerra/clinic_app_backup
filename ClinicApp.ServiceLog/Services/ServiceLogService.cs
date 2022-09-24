using ClinicApp.MSServiceLog.Interfaces;
using ClinicApp.Core.Data;
using ClinicApp.Core.Interfaces;
using ClinicApp.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClinicApp.MSServiceLog.Models;

namespace ClinicApp.MSServiceLog.Services;

public class ServiceLogService : IServiceLog
{
    private readonly clinicbdContext _context;
    private readonly IUriService _uriService;
    public ServiceLogService(clinicbdContext context, IUriService uriService)
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

        _context.ServiceLogs.Remove(serviceLog);
        await _context.SaveChangesAsync();

        return new ServiceLog { };
    }

    public async Task<PagedResponse<IEnumerable<ServiceLog>>> GetServiceLog([FromQuery] PaginationFilter filter, string route)
    {
        var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
        var list = await _context.ServiceLogs.Include("Client").Include("Contractor").Include("Period")
            .AsNoTracking()
            .OrderBy(x => x.Period.StartDate)
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
            UnitDetails = _context.UnitDetails.Include("SubProcedure").Include("PlaceOfService").Where(ud => ud.ServiceLogId == x.Id).ToList(),
            Contractor = x.Contractor,
            ContractorId = x.ContractorId,
            Client = x.Client,
            ClientId = x.ClientId
        }).Where(x => x.Id == serviceLog.Id).FirstOrDefaultAsync();
    }

    public async Task<PagedResponse<IEnumerable<ServiceLog>>> GetServiceLogByName([FromQuery] PaginationFilter filter, string name, string route, string type)
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
            return pagedReponse;
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
            return pagedReponse;
        }
    }

    public async Task<IEnumerable<ServiceLogWithoutDetailsDto>> GetServiceLogWithoutDetails()
    {
        var list = await(from sl in _context.ServiceLogs
                         select new ServiceLogWithoutDetailsDto
                         {
                             ClientName = sl.Client.Name,
                             ContractorName = sl.Contractor.Name,
                             StartDate = sl.Period.StartDate,
                             EndDate = sl.Period.EndDate
                         }).ToListAsync();
        return list;
    }

    public async Task<ServiceLog?> PostServiceLog(ServiceLog serviceLog)
    {
        _context.ServiceLogs.Add(serviceLog);

        await _context.SaveChangesAsync();

        return await _context.ServiceLogs.Include("Contractors").Include("Clients").Include("Periods").Where(x => x.Id == serviceLog.Id).FirstOrDefaultAsync();
    }

    public async Task<object?> PutServiceLog(int id, ServiceLog serviceLog, bool partial = true)
    {

        var oldServiceLog = await _context.ServiceLogs.Include("UnitDetails").Where(x => x.Id == id).FirstOrDefaultAsync();
        // Add olds
        foreach (var olds in oldServiceLog!.UnitDetails)
        {
            var found = false;

            foreach (var news in serviceLog.UnitDetails)
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

        return new ServiceLog {};
    }

    public bool ServiceLogExists(int id)
    {
        return _context.ServiceLogs.Any(e => e.Id == id);
    }
}
