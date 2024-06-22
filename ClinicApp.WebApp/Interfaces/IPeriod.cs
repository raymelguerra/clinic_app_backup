using ClinicApp.Core.Models;

namespace ClinicApp.WebApp.Interfaces;

public interface IPeriod 
{ 
    public Task<IEnumerable<Period>> GetPeriodAsync(string filter);
    public Task<Period?> GetPeriodAsync(int id);
    public Task<bool> PutPeriodAsync(int id, Period Period);
    public Task<bool> PostPeriodAsync(Period Period);
    public Task<bool> DeletePeriodAsync(int id);
}
