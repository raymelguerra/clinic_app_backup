using ClinicApp.Core.Models;

namespace ClinicApp.WebApp.Interfaces;

public interface IPlaceOfService
{
    public Task<IEnumerable<PlaceOfService>> GetPlaceOfServiceAsync(string filter);
    public Task<PlaceOfService?> GetPlaceOfServiceAsync(int id);
    public Task<bool> PutPlaceOfServiceAsync(int id, PlaceOfService PlaceOfService);
    public Task<bool> PostPlaceOfServiceAsync(PlaceOfService PlaceOfService);
    public Task<bool> DeletePlaceOfServiceAsync(int id);
}
