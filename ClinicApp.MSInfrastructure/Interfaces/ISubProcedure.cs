using ClinicApp.Core.Models;

namespace ClinicApp.Infrastructure.Interfaces;

public interface ISubProcedure
{
    public Task<IEnumerable<SubProcedure>> Get();
    public Task<SubProcedure?> Get(int id);
}
