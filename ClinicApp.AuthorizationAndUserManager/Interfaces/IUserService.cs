using ClinicApp.AuthorizationAndUserManager.Models;

namespace ClinicApp.AuthorizationAndUserManager.Interfaces;
public interface IUserService
{

    public Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model);

    public Task<UserManagerResponse> LoginUserAsync(LoginViewModel model);
    // Task<UserManagerResponse> ConfirmEmailAsync(string userId, string token);

    // Task<UserManagerResponse> ForgetPasswordAsync(string email);

    public Task<UserManagerResponse> ResetPasswordAsync(ResetPasswordViewModel model);
    public Task<UserManagerResponse> DeleteUserAsync(string id);
    public Task<IEnumerable<UserViewModel>> GetAllUsersAsync();
}

