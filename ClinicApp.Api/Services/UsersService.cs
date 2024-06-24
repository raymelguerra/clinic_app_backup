using AutoMapper;
using ClinicApp.Core.Dtos;
using ClinicApp.Infrastructure.Interfaces;
using Oauth2.sdk;

namespace ClinicApp.Api.Services
{
    public class UsersService(IUserManagementService userManagement, IRolesManagementService rolesManagementService, IMapper mapper) : IUsersService
    {
        private readonly IUserManagementService _userManagement = userManagement;
        private readonly IRolesManagementService _rolesManagementService = rolesManagementService;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<UserVM>> GetAllUsers()
        {
            var oauthUserList = await _userManagement.GetUsers();
            if (oauthUserList is null)
                return Enumerable.Empty<UserVM>();

            var userList = oauthUserList
                .Select(x => _mapper.Map<UserVM>(x))
                .ToList();

            return userList;
        }

        public async Task<UserVM?> GetUser(string userId)
        {
            var userOauth = await _userManagement.GetUserData(userId);
            if (userOauth is null)
                return null;

            var user = _mapper.Map<UserVM>(userOauth);

            user.Roles = await _rolesManagementService.GetRolesByUser(userId);

            return user;
        }

        public async Task<bool> CreateUser(UserVM user)
        {
            var userId = await _userManagement.AddUser(
                _mapper.Map<Oauth2.sdk.Models.User>(user));

            if (string.IsNullOrEmpty(userId))
                return false;

            return true;
        }

        public async Task<bool> UpdateUser(string userId, UserVM user)
        {
            var result = await _userManagement.UpdateUser(userId,
                _mapper.Map<Oauth2.sdk.Models.User>(user));

            if (!result)
                return false;
            return result;
        }

        public async Task<bool> DeleteUser(string userId)
        {
            var result = await _userManagement.RemoveUser(userId);

            if (!result)
                return false;

            return true;
        }
    }
}
