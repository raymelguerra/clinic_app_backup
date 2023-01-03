﻿using ClinicApp.MSClient.Interfaces;
using ClinicApp.Core.Data;
using ClinicApp.Core.Interfaces;
using ClinicApp.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicApp.MSClient.Services;

public class ClientService : IClient
{
    private readonly ClinicbdMigrationContext _context;
    private readonly IUriService _uriService;
    public ClientService(ClinicbdMigrationContext context, IUriService uriService)
    {
        _context = context;
        _uriService = uriService;
    }

    public bool ClientExists(int id)
    {
        return _context.Clients.Any(e => e.Id == id);
    }

    public async Task<object?> DeleteClient(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client == null)
        {
            return null;
        }

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();

        return new Client { };
    }

    public async Task<PagedResponse<IEnumerable<Client>>> GetClient([FromQuery] PaginationFilter filter, string route)
    {
        var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
        var list = await _context.Clients
            .Include(x => x.ReleaseInformation)
            .Include(x => x.Diagnosis)
            .Select(c => new Client
            {
                Id = c.Id,
                Diagnosis = c.Diagnosis,
                DiagnosisId = c.DiagnosisId,
                Agreements = new List<Agreement>(), //.Agreement.Include(x => x.Payroll.Contractor).Where(a => a.ClientId == c.Id).ToList(),
                AuthorizationNumber = c.AuthorizationNumber,
                Enabled = c.Enabled,
                Name = c.Name,
                PatientAccount = c.PatientAccount,
                RecipientId = c.RecipientId,
                ReferringProvider = c.ReferringProvider,
                ReleaseInformation = c.ReleaseInformation,
                ReleaseInformationId = c.ReleaseInformationId,
                Sequence = c.Sequence,
                WeeklyApprovedAnalyst = c.WeeklyApprovedAnalyst,
                WeeklyApprovedRbt = c.WeeklyApprovedRbt,
                ServiceLogs = c.ServiceLogs
            })
            .OrderBy(o => o.Id)
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToListAsync();

        var totalRecords = await _context.Clients.CountAsync();

        var pagedReponse = PaginationHelper.CreatePagedReponse<Client>(list, validFilter, totalRecords, _uriService, route);
        return pagedReponse;
    }

    public async Task<Client?> GetClient(int id)
    {
        var client = await _context.Clients.Include(x => x.ReleaseInformation).Include(x => x.Diagnosis).Select(c => new Client
        {
            Id = c.Id,
            Name = c.Name,
            PatientAccount = c.PatientAccount,
            RecipientId = c.RecipientId,
            ReferringProvider = c.ReferringProvider,
            ReleaseInformation = c.ReleaseInformation,
            ReleaseInformationId = c.ReleaseInformationId,
            Sequence = c.Sequence,
            WeeklyApprovedAnalyst = c.WeeklyApprovedAnalyst,
            WeeklyApprovedRbt = c.WeeklyApprovedRbt,
            AuthorizationNumber = c.AuthorizationNumber,
            Diagnosis = c.Diagnosis,
            DiagnosisId = c.DiagnosisId,
            Enabled = c.Enabled,
            Agreements = _context.Agreements.Include("Payroll").Include("Company").Select(ag => new Agreement
            {
                Payroll = _context.Payrolls.Include("Procedure").Include("ContractorType").Include("Contractor").Where(p => p.Id == ag.PayrollId).First(),
                ClientId = ag.ClientId,
                Company = ag.Company,
                CompanyId = ag.CompanyId,
                PayrollId = ag.PayrollId,
                RateEmployees = ag.RateEmployees
            }).Where(a => a.ClientId == id).ToList()

        }).FirstOrDefaultAsync(x => x.Id == id);

        if (client == null)
        {
            return null;
        }

        return client;
    }

    public async Task<PagedResponse<IEnumerable<Client>>> GetClientByName([FromQuery] PaginationFilter filter, string name, string route)
    {
        var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
        var list = await _context.Clients
            .Include(x => x.ReleaseInformation)
            .Include(x => x.Diagnosis)
            .Select(c => new Client
            {
                Id = c.Id,
                Diagnosis = c.Diagnosis,
                DiagnosisId = c.DiagnosisId,
                Agreements = new List<Agreement>(),
                AuthorizationNumber = c.AuthorizationNumber,
                Enabled = c.Enabled,
                Name = c.Name,
                PatientAccount = c.PatientAccount,
                RecipientId = c.RecipientId,
                ReferringProvider = c.ReferringProvider,
                ReleaseInformation = c.ReleaseInformation,
                ReleaseInformationId = c.ReleaseInformationId,
                Sequence = c.Sequence,
                WeeklyApprovedAnalyst = c.WeeklyApprovedAnalyst,
                WeeklyApprovedRbt = c.WeeklyApprovedRbt,
                ServiceLogs = new List<ServiceLog>()
            })
            .Where(x => x.Name!.ToUpper().Contains(name.ToUpper()))
            .OrderBy(o => o.Id)
            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToListAsync();

        var totalRecords = list.Count();

        var pagedReponse = PaginationHelper.CreatePagedReponse(list, validFilter, totalRecords, _uriService, route);
        return pagedReponse;
    }

    public async Task<IEnumerable<Client>> GetClientsByContractor(int id)
    {
        var list = await (from a in _context.Agreements
                          join p in _context.Payrolls on a.PayrollId equals p.Id
                          join c in _context.Clients on a.ClientId equals c.Id
                          where p.ContractorId == id
                          select new { Id = c.Id, Name = c.Name }).Distinct().ToListAsync();
        return (IEnumerable<Client>)list;
    }

    public async Task<IEnumerable<Client>> GetClientWithoutDetails()
    {
        var clients = await _context.Clients.Select(c => new Client
        {
            Id = c.Id,
            Name = c.Name
        }).OrderBy(o => o.Id).ToListAsync();
        
        return clients;
    }

    public async Task<Client?> PostClient(Client client)
    {
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();

        var client_new = await _context.Clients.Include(x => x.ReleaseInformation).Include(x => x.Diagnosis).Where(c => c.Id == client.Id)
                .Select(c => new Client
                {
                    Id = c.Id,
                    Diagnosis = c.Diagnosis,
                    DiagnosisId = c.DiagnosisId,
                    Agreements = _context.Agreements.Include(x => x.Payroll.Contractor).Where(a => a.ClientId == c.Id).ToList(),
                    AuthorizationNumber = c.AuthorizationNumber,
                    Enabled = c.Enabled,
                    Name = c.Name,
                    PatientAccount = c.PatientAccount,
                    RecipientId = c.RecipientId,
                    ReferringProvider = c.ReferringProvider,
                    ReleaseInformation = c.ReleaseInformation,
                    ReleaseInformationId = c.ReleaseInformationId,
                    Sequence = c.Sequence,
                    WeeklyApprovedAnalyst = c.WeeklyApprovedAnalyst,
                    WeeklyApprovedRbt = c.WeeklyApprovedRbt
                }).FirstOrDefaultAsync(x => x.Id == client.Id);
        
        return client_new;
    }

    public async Task<object?> PutClient(int id, Client client)
    {
        var clientOld = await _context.Clients.Include(p => p.Agreements).FirstOrDefaultAsync(p => p.Id == client.Id);
        clientOld!.Agreements.Clear();

        foreach (var item in client.Agreements)
        {
            clientOld.Agreements.Add(item);
        }
        clientOld.AuthorizationNumber = client.AuthorizationNumber;
        clientOld.Name = client.Name;
        clientOld.RecipientId = client.RecipientId;
        clientOld.PatientAccount = client.PatientAccount;
        clientOld.ReleaseInformationId = client.ReleaseInformationId;
        clientOld.ReleaseInformation = client.ReleaseInformation;
        clientOld.ReferringProvider = client.ReferringProvider;
        clientOld.Sequence = client.Sequence;
        clientOld.WeeklyApprovedRbt = client.WeeklyApprovedRbt;
        clientOld.WeeklyApprovedAnalyst = client.WeeklyApprovedAnalyst;
        clientOld.DiagnosisId = client.DiagnosisId;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ClientExists(id))
            {
                return null;
            }
            else
            {
                throw;
            }
        }
        return new Client();
    }
}
