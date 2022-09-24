using ClinicApp.Core.Models;

namespace ClinicApp.Infrastructure.Interfaces;

public interface IProcedure
{
    public Task<IEnumerable<Procedure>> Get();
    public Task<Procedure?> Get(int id);
}
