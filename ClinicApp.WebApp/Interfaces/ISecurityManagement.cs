using ClinicApp.Core.Dtos;
using Oauth2.sdk.Models;

namespace ClinicApp.WebApp.Interfaces
{
    public interface ISecurityManagement
    {
        Task<bool> CreateRoleAsync(Role role);
        Task<bool> CreateUserAsync(UserVM user);
        void Dispose();
        Task<IEnumerable<Role>?> GetRolesAsync(string filter = "");
        Task<UserVM?> GetUserAsync(string id);
        Task<IEnumerable<string>?> GetUserRolesByUserNameAsync(string username);
        Task<bool> UpdateUserRolesByUserAsync(UserVM user, string username);
        Task<IEnumerable<UserVM>?> GetUsersAsync(string filter = "");
        IAsyncEnumerable<UserVM?> GetUsersStream(string filter = "");
        Task<bool> RemoveRoleAsync(int roleId);
        Task<bool> RemoveUserAsync(string id);
        Task<bool> UpdateRolesAsync(Role role, int roleId);
        Task<bool> UpdateUserAsync(UserVM user, string id, bool inDb = false);
    }
}