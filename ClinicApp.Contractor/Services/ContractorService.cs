using ClinicApp.MSContractor.Interfaces;
using ClinicApp.Core.Data;
using ClinicApp.Core.Interfaces;
using ClinicApp.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicApp.MSContractor.Services;

public class ContractorService : IContractor
{
    private readonly clinicbdContext _context;
    private readonly IUriService _uriService;
    public ContractorService(clinicbdContext context, IUriService uriService)
    {
        _context = context;
        _uriService = uriService;
    }

    public bool ContractorExists(int id)
    {
        return _context.Contractors.Any(e => e.Id == id);
    }

    public async Task<object?> DeleteContractor(int id)
    {
        var contractor = await _context.Contractors.FindAsync(id);
        if (contractor == null)
        {
            return null;
        }

        _context.Contractors.Remove(contractor);
        await _context.SaveChangesAsync();

        return new Contractor { };
    }

    public async Task<PagedResponse<IEnumerable<Contractor>>> GetContractor([FromQuery] PaginationFilter filter, string route)
    {
        var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

        var list = await _context.Contractors.Select(x => new Contractor
        {
            Id = x.Id,
            Name = x.Name,
            Extra = x.Extra,
            Payrolls = x.Payrolls.Select(y => new Payroll
            {
                ContractorType = y.ContractorType,
                Procedure = y.Procedure,
                Company = y.Company,
                Id = y.Id
            })
                    .ToList(),
            RenderingProvider = x.RenderingProvider
        })
            .OrderBy(o => o.Id)
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToListAsync();


        var totalRecords = await _context.Contractors.CountAsync();

        var pagedReponse = PaginationHelper.CreatePagedReponse<Contractor>(list, validFilter, totalRecords, _uriService, route);
        return pagedReponse;
    }

    public async Task<Contractor?> GetContractor(int id)
    {
        var contractor = await _context.Contractors.Where(x => x.Id == id).Select(x => new Contractor
        {
            Id = x.Id,
            Name = x.Name,
            Extra = x.Extra,
            Payrolls = x.Payrolls.Select(y => new Payroll { ContractorType = y.ContractorType, Procedure =  y.Procedure, Company =  y.Company }).ToList(),
            RenderingProvider = x.RenderingProvider,
        }).FirstOrDefaultAsync();

        if (contractor == null)
        {
            return null;
        }

        return contractor;
    }

    public async Task<Contractor?> PostContractor(Contractor contractor)
    {
        _context.Contractors.Add(contractor);

        await _context.SaveChangesAsync();

        var list = await _context.Contractors.Where(z => z.Id == contractor.Id).Select(x => new Contractor
        {
            Id = x.Id,
            Name = x.Name,
            Extra = x.Extra,
            Payrolls = x.Payrolls.Select(y => new Payroll { ContractorType = y.ContractorType, Procedure = y.Procedure, Company = y.Company }).ToList(),
            RenderingProvider = x.RenderingProvider
        }).FirstOrDefaultAsync();

        return list;
    }

    public async Task<object?> PutContractor(int id, Contractor contractor, bool partial = true)
    {
        var contractorOld = await _context.Contractors.Include(p => p.Payrolls).FirstOrDefaultAsync(p => p.Id == contractor.Id);

        if (!partial)
        {
            // Verify if analyst have agreement
            var relations = await (from ag in _context.Agreements
                                   where ag.Payroll.ContractorId == id
                                   select ag.Client).ToListAsync();

            if (relations.Count() > 0)
            {
                return null;
            }

            contractorOld!.Payrolls.Clear();

            foreach (var item in contractor.Payrolls)
            {
                contractorOld.Payrolls.Add(item);
            }
        }

        contractorOld!.Name = contractor.Name;
        contractorOld.RenderingProvider = contractor.RenderingProvider;
        contractorOld.Extra = contractor.Extra;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ContractorExists(id))
            {
                return null;
            }
            else
            {
                throw;
            }
        }

        return new Contractor();
    }

    public async Task<IEnumerable<Contractor>> GetContractorWithoutDetails()
    {
        var list = await _context.Contractors.Select(c => new Contractor() { Id = c.Id, Name = c.Name }).OrderBy(o => o.Id).ToListAsync();
        return list;
    }

    public async Task<PagedResponse<IEnumerable<Contractor>>> GetContractorByName([FromQuery] PaginationFilter filter, string name, string route)
    {
        var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

        var list = await _context.Contractors.Select(x => new Contractor
        {
            Id = x.Id,
            Name = x.Name,
            Extra = x.Extra,
            Payrolls = x.Payrolls.Select(y => new Payroll
            {
                ContractorType = y.ContractorType,
                Procedure = y.Procedure,
                Company = y.Company,
                Id = y.Id
            })
                    .ToList(),
            RenderingProvider = x.RenderingProvider
        })
            .Where(x => x.Name!.ToUpper().Contains(name.ToUpper()))
            .OrderBy(o => o.Id)
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToListAsync();

        var totalRecords = list.Count();

        var pagedReponse = PaginationHelper.CreatePagedReponse<Contractor>(list, validFilter, totalRecords, _uriService, route);
        return pagedReponse;
    }

    public async Task<IEnumerable<Contractor>> GetAnalystByCompany(int id)
    {
        var contractor = await(from pr in _context.Payrolls
                               where pr.CompanyId == id && pr.ContractorType.Name == "Analyst"
                               select pr.Contractor).ToListAsync();

        return contractor;
    }

    public async Task<IEnumerable<Contractor>> GetContractorByCompany(int id)
    {
        var contractor = await(from pr in _context.Payrolls
                               where pr.CompanyId == id
                               select pr.Contractor).ToListAsync();

        return contractor;
    }

    public async Task<IEnumerable<Payroll>> GetPayroll()
    {
        return await _context.Payrolls.Include("Procedure").Include("ContractorType").ToListAsync();
    }

    public async Task<IEnumerable<Payroll>> GetPayrollsByContractorAndCompany(int idCo, int idCont)
    {
        return await _context.Payrolls.Include("Procedure").Include("ContractorType").Where(x => x.CompanyId == idCo && x.ContractorId == idCont).ToListAsync();
    }

    public async Task<Payroll?> GetPayroll(int id)
    {
        var payroll = await _context.Payrolls.FindAsync(id);

        if (payroll == null)
        {
            return null;
        }

        return payroll;
    }

    public async Task<object?> PutPayroll(int id, Payroll payroll)
    {
        _context.Entry(payroll).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PayrollExists(id))
            {
                return null;
            }
            else
            {
                throw;
            }
        }

        return new Payroll { };
    }

    public async Task<Payroll?> PostPayroll(Payroll payroll)
    {
        _context.Payrolls.Add(payroll);
        await _context.SaveChangesAsync();

        return await GetPayroll(payroll.Id);
    }

    public async Task<object?> DeletePayroll(int id)
    {
        var payroll = await _context.Payrolls.FindAsync(id);
        if (payroll == null)
        {
            return null;
        }

        _context.Payrolls.Remove(payroll);
        await _context.SaveChangesAsync();

        return new Payroll { };
    }

    public bool PayrollExists(int id)
    {
        return _context.Payrolls.Any(e => e.Id == id);
    }
}
