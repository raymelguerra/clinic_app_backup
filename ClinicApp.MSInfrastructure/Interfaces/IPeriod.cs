using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClinicApp.Infrastructure.Interfaces;

public interface IPeriod
{
    public Task<IEnumerable<Period>> Get();
    public Task<Period?> Get(int id);
    public Task<Period?> GetActivePeriod();
    public Task<DataPeriodDto?> GetDataPeriod(int id_period, int id_client);
}
