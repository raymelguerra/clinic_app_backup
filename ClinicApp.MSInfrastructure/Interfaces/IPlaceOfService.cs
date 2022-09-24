using ClinicApp.Core.Models;

namespace ClinicApp.Infrastructure.Interfaces;

public interface IPlaceOfService
{
    public Task<IEnumerable<PlaceOfService>> Get();
    public Task<PlaceOfService?> Get(int id);
}
