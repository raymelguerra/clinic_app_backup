using ClinicApp.Core.Data;
using ClinicApp.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClinicApp.MSClient.Interfaces;
public interface IClient
{
    public Task<PagedResponse<IEnumerable<Client>>> GetClient([FromQuery] PaginationFilter filter, string route);
    public Task<Client?> GetClient(int id);
    public Task<IEnumerable<Client>> GetClientWithoutDetails();
    public Task<PagedResponse<IEnumerable<Client>>> GetClientByName([FromQuery] PaginationFilter filter, string name, string route);
    public Task<IEnumerable<Client>> GetClientsByContractor(int id);
    public Task<object?> PutClient(int id, Client biller);
    public Task<Client?> PostClient(Client biller);
    public Task<object?> DeleteClient(int id);
    public bool ClientExists(int id);
}
