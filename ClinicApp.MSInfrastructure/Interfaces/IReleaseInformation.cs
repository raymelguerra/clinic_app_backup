using ClinicApp.Core.Models;

namespace ClinicApp.Infrastructure.Interfaces;

public interface IReleaseInformation
{
    public Task<IEnumerable<ReleaseInformation>> Get();
    public Task<ReleaseInformation?> Get(int id);
}
