using ClinicApp.Core.Models;

namespace ClinicApp.WebApp.Interfaces
{
    public interface ICompany
    {
        public Task<IEnumerable<Company>> GetCompanyAsync(string filter);
        public Task<Company?> GetCompanyAsync(int id);
    }
}
