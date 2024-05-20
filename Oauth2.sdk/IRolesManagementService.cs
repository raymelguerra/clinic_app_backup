using Oauth2.sdk.Models;

namespace Oauth2.sdk
{
    public interface IRolesManagementService
    {
        Task<bool> AddRole(Role role);
        Task<IEnumerable<Role>?> GetListClientRoles();
        Task<Role?> GetRoleData(string id);
        Task<IEnumerable<Role>?> GetRolesByUser(string userId);
        List<string> GetUserRoles();
        bool IsInRole(string role);
        Task<bool> RemoveRole(string roleId);
        Task<bool> UpdateRole(Role role);
    }
}