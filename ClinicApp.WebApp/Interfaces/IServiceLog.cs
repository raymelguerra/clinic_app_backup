using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Dto.Application;
using Microsoft.AspNetCore.Mvc;

namespace ClinicApp.WebApp.Interfaces;

public interface IServiceLog
{
    public Task<IEnumerable<ServiceLog>> GetServiceLogAsync(string filter);
    public Task<ServiceLog?> GetServiceLogAsync(int id);
    public Task<bool> PutServiceLogAsync(int id, ServiceLog ServiceLog);
    public Task<bool> PostServiceLogAsync(ServiceLog ServiceLog);
    public Task<bool> DeleteServiceLogAsync(int id);
    public Task<IEnumerable<ServiceLog>> GetAllServiceLogsByClientContractorPeriodInsurance(int clientId, int contractorId, int periodId, int insuranceId);

    public Task<PeriodCalculationResultDto> CalculatePeriodAsync(int periodId);
}
