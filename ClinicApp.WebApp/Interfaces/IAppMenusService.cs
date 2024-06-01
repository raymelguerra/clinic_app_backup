using ClinicApp.Core.Models;

namespace ClinicApp.WebApp.Interfaces
{
    public interface IAppMenusService
    {
        Task<IEnumerable<ParentMenu>> GetMenusAsync();
    }
}