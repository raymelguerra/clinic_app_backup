using ClinicApp.Core.Data;
using ClinicApp.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClinicApp.MSContractor.Interfaces;
public interface IContractor
{
    public Task<PagedResponse<IEnumerable<Contractor>>> GetContractor([FromQuery] PaginationFilter filter, string route);
    public Task<Contractor?> GetContractor(int id);
    public Task<IEnumerable<Contractor>> GetContractorWithoutDetails();
    public Task<PagedResponse<IEnumerable<Contractor>>> GetContractorByName([FromQuery] PaginationFilter filter, string name, string route);
    public Task<IEnumerable<Contractor>> GetAnalystByCompany(int id);
    public Task<IEnumerable<Contractor>> GetContractorByCompany(int id);
    public Task<object?> PutContractor(int id, Contractor contractor, bool partial = true);
    public Task<Contractor?> PostContractor(Contractor contractor);
    public Task<object?> DeleteContractor(int id);
    public bool ContractorExists(int id);
}
