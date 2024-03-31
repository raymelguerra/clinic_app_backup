using ClinicApp.Core.Models;

namespace ClinicApp.WebApp.Interfaces
{
    public interface IReleaseInformation
    {
        public Task<IEnumerable<ReleaseInformation>> GetReleaseInformationAsync(string filter);
        public Task<ReleaseInformation?> GetReleaseInformationAsync(int id);
    }
}
