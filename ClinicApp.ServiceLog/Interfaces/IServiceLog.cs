using ClinicApp.Core.Data;
using ClinicApp.Core.Models;
using ClinicApp.MSServiceLog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ClinicApp.MSServiceLog.Interfaces;
public interface IServiceLog
{
    public Task<PagedResponse<IEnumerable<ServiceLog>>> GetServiceLog(PaginationFilter filter, string route);
    public Task<ServiceLog?> GetServiceLog(int id);
    public Task<IEnumerable<ServiceLogWithoutDetailsDto>> GetServiceLogWithoutDetails();
    public Task<PagedResponse<IEnumerable<ServiceLog>>> GetServiceLogByName(PaginationFilter filter, string name, string route, string type);
    public Task<object?> PutServiceLog(int id, ServiceLog serviceLog, bool partial = true);
    public Task<ServiceLog?> PostServiceLog(ServiceLog serviceLog);
    public Task<object?> DeleteServiceLog(int id);
    public Task<PagedResponse<IEnumerable<ServiceLog?>>> GetPendingServiceLog(PaginationFilter filter, string route);
    public Task<ServiceLog?> UpdatePendingStatus(int id);
    public Task<PagedResponse<IEnumerable<ServiceLog?>>> GetServiceLogsByName(PaginationFilter filter, string name, string type, string route);
    public bool ServiceLogExists(int id);
}
