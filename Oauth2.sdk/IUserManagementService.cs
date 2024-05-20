using Oauth2.sdk.Models;

namespace Oauth2.sdk
{
    public interface IUserManagementService
    {
        string? GetToken();
        string? GetUserFullNames();
        Task<User?> GetUserData(string id);
        Task<bool> UpdateUser(string userId, User user);
        Task<string> AddUser(User user);
        List<string> GetUserRoles();
        Task<IEnumerable<User>?> GetUsers();
        Task<bool> RemoveUser(string userId);
        string? GetUserName();
        string? GetUserId();
        Task<bool> GrantUserChangePasswordAsync(string userId);
        User GetUserContextMainData();
    }
}