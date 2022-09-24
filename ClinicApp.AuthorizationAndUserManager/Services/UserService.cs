using ClinicApp.AuthorizationAndUserManager.Interfaces;
using ClinicApp.AuthorizationAndUserManager.Models;
using ClinicApp.Core.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ClinicApp.AuthorizationAndUserManager.Services;
public class UserService : IUserService
{

    private UserManager<IdentityUser> _userManager;
    private IConfiguration _configuration;
    private JwtHandler _jwtHandler;

    public UserService(UserManager<IdentityUser> userManager, IConfiguration configuration, JwtHandler jwtHandler)
    {
        _userManager = userManager;
        _configuration = configuration;
        _jwtHandler = jwtHandler;
    }

    public async Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model)
    {
        if (model == null)
            throw new NullReferenceException("Register Model is null");

        if (model.Password != model.ConfirmPassword)
            return new UserManagerResponse
            {
                Message = "Confirm password doesn't match the password",
                IsSuccess = false,
            };

        var identityUser = new IdentityUser
        {
            Email = model.Email,
            UserName = model.Username,
        };


        var result = await _userManager.CreateAsync(identityUser, model.Password);
        if (!result.Succeeded)
        {
            return new UserManagerResponse
            {
                Message = "User did not create",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        var user = await _userManager.FindByNameAsync(model.Username);
        foreach (var rol in model.Rol)
        {
            await _userManager.AddToRoleAsync(user, rol);
        }

        if (result.Succeeded)
        {
            return new UserManagerResponse
            {
                Message = "User created successfully!",
                IsSuccess = true,
            };
        }

        return new UserManagerResponse
        {
            Message = "User did not create",
            IsSuccess = false,
            Errors = result.Errors.Select(e => e.Description)
        };

    }

    public async Task<UserManagerResponse> LoginUserAsync(LoginViewModel model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);

        if (user == null)
        {
            return new UserManagerResponse
            {
                Message = "There is no user with that Username",
                IsSuccess = false,
            };
        }

        var result = await _userManager.CheckPasswordAsync(user, model.Password);

        if (!result)
            return new UserManagerResponse
            {
                Message = "Invalid password",
                IsSuccess = false,
            };

        var roles_list = await _userManager.GetRolesAsync(user);

        List<Claim> claims = new List<Claim>();
        foreach (var rol in roles_list)
        {
            claims.Add(new Claim(ClaimTypes.Role, rol));
        }
        claims.Add(new Claim("Username", model.Username));
        claims.Add(new Claim("Email", user.Email));
        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["AuthSettings:Issuer"],
            audience: _configuration["AuthSettings:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

        return new UserManagerResponse
        {
            Message = tokenAsString,
            IsSuccess = true,
            ExpireDate = token.ValidTo
        };
    }

    public async Task<UserManagerResponse> ConfirmEmailAsync(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return new UserManagerResponse
            {
                IsSuccess = false,
                Message = "User not found"
            };

        var decodedToken = WebEncoders.Base64UrlDecode(token);
        string normalToken = Encoding.UTF8.GetString(decodedToken);

        var result = await _userManager.ConfirmEmailAsync(user, normalToken);

        if (result.Succeeded)
            return new UserManagerResponse
            {
                Message = "Email confirmed successfully!",
                IsSuccess = true,
            };

        return new UserManagerResponse
        {
            IsSuccess = false,
            Message = "Email did not confirm",
            Errors = result.Errors.Select(e => e.Description)
        };
    }

    public async Task<UserManagerResponse> ForgetPasswordAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return new UserManagerResponse
            {
                IsSuccess = false,
                Message = "No user associated with email",
            };

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = Encoding.UTF8.GetBytes(token);
        var validToken = WebEncoders.Base64UrlEncode(encodedToken);

        string url = $"{_configuration["AppUrl"]}/ResetPassword?email={email}&token={validToken}";

        // AQUI VAN COSAS

        return new UserManagerResponse
        {
            IsSuccess = true,
            Message = "Reset password URL has been sent to the email successfully!"
        };
    }

    public async Task<UserManagerResponse> ResetPasswordAsync(ResetPasswordViewModel model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user == null)
            return new UserManagerResponse
            {
                IsSuccess = false,
                Message = "No user associated with email",
            };

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

        if (result.Succeeded)
            return new UserManagerResponse
            {
                Message = "Password has been reset successfully!",
                IsSuccess = true,
            };

        return new UserManagerResponse
        {
            Message = "Something went wrong",
            IsSuccess = false,
            Errors = result.Errors.Select(e => e.Description),
        };
    }

    public async Task<UserManagerResponse> DeleteUserAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return new UserManagerResponse
            {
                Message = "There is no user with that Id",
                IsSuccess = false,
            };
        }
        var roles_list = await _userManager.GetRolesAsync(user);

        var result = await _userManager.RemoveFromRolesAsync(user, roles_list);

        if (!result.Succeeded)
        {
            return new UserManagerResponse
            {
                Message = "Something went wrong",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description),
            };
        }

        result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            return new UserManagerResponse
            {
                Message = "Something went wrong",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description),
            };
        }
        return new UserManagerResponse
        {
            Message = "User has been deleted successfully!",
            IsSuccess = true,
        };

    }

    public async Task<IEnumerable<UserViewModel>> GetAllUsersAsync()
    {
        var result = new List<UserViewModel>();

        var list = await GetIdentityUsersAsync();

        foreach(var item in list)
        {
            var role = await _userManager.GetRolesAsync(item);
            result.Add(new UserViewModel
            {
                username = item.UserName,
                email = item.Email,
                id = item.Id,
                roles = role.ToList()
            });
        }
        return result;
    }

    public async Task<List<IdentityUser>> GetIdentityUsersAsync()
    {
        return await Task.Run(() =>
        {
            return _userManager.Users.ToList();
        });
    }
}
