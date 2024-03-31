using ClinicApp.Core.Models;

namespace ClinicApp.WebApp.Interfaces;

public interface IProcedure
{
    public Task<IEnumerable<Procedure>> GetProcedureAsync(string filter);
    public Task<Procedure?> GetProcedureAsync(int id);
    public Task<bool> PutProcedureAsync(int id, Procedure procedure);
    public Task<bool> PostProcedureAsync(Procedure procedure);
    public Task<bool> DeleteProcedureAsync(int id);
}
