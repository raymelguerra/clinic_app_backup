
using ClinicApp.Core.Dtos;
using ClinicApp.Infrastructure.Dto;

namespace ClinicApp.Infrastructure.Interfaces;

public interface IUsersService
{
    Task<bool> CreateUser(UserVM user);
    Task<bool> DeleteUser(string userId);
    Task<IEnumerable<UserVM>> GetAllUsers();
    Task<UserVM?> GetUser(string userId);
    Task<bool> UpdateUser(string userId, UserVM user);
    Task<bool> ChangePassword(string userId, ChangePasswordDto newPassword);
}
