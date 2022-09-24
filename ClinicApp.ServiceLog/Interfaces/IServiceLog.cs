using ClinicApp.Core.Data;
using ClinicApp.Core.Models;
using ClinicApp.MSServiceLog.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClinicApp.MSServiceLog.Interfaces;
public interface IServiceLog
{
    public Task<PagedResponse<IEnumerable<ServiceLog>>> GetServiceLog([FromQuery] PaginationFilter filter, string route);
    public Task<ServiceLog?> GetServiceLog(int id);
    public Task<IEnumerable<ServiceLogWithoutDetailsDto>> GetServiceLogWithoutDetails();
    public Task<PagedResponse<IEnumerable<ServiceLog>>> GetServiceLogByName([FromQuery] PaginationFilter filter, string name, string route, string type);
    public Task<object?> PutServiceLog(int id, ServiceLog serviceLog, bool partial = true);
    public Task<ServiceLog?> PostServiceLog(ServiceLog serviceLog);
    public Task<object?> DeleteServiceLog(int id);
    public bool ServiceLogExists(int id);
}
