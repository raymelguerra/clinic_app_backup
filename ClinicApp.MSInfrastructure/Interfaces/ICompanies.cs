using ClinicApp.Core.Models;

namespace ClinicApp.Infrastructure.Interfaces
{
    public interface ICompanies
    {
        public Task<IEnumerable<Company>> Get();
        public Task<Company?> Get(int id);
    }
}
