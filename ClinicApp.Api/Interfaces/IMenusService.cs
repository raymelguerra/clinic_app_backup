using ClinicApp.Core.Models;

namespace ClinicApp.Api.Interfaces;

public interface IMenusService
{
    List<ParentMenu>? GetMenusByRole(string role);
}
