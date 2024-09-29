using ClinicApp.Core.Models;

namespace ClinicApp.WebApp.Interfaces;

public interface IServiceLog
{
    public Task<IEnumerable<ServiceLog>> GetServiceLogAsync(string filter);
    public Task<ServiceLog?> GetServiceLogAsync(int id);
    public Task<bool> PutServiceLogAsync(int id, ServiceLog ServiceLog);
    public Task<bool> PostServiceLogAsync(ServiceLog ServiceLog);
    public Task<bool> DeleteServiceLogAsync(int id);
}
