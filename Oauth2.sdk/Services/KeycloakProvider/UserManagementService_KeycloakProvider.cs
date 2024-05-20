using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Oauth2.sdk.Exceptions;
using Oauth2.sdk.Models;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Oauth2.sdk.Services.KeycloakProvider
{
    public class UserManagementService_KeycloakProvider(
        IOptions<CredentialsSettings> _options,
        IHttpContextAccessor _httpContextAccessor
        ) : KeycloakProviderBase(_options, _httpContextAccessor), IUserManagementService
    {
        public async Task<IEnumerable<User>?> GetUsers()
        {
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var response = await client.GetAsync(
                $"{credentials.Authority}admin/realms/{credentials.Realm}/users");

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();

            if (response.StatusCode == HttpStatusCode.Forbidden)
                throw new ForbiddenAccessException();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error: {response.StatusCode}");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<User>>(
                result);
        }

        public User GetUserContextMainData()
        {
            return new User
            {
                Username = context.HttpContext?.User.Claims.First(claim => claim.Type == "preferred_username").Value,
                FirstName = context.HttpContext?.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.GivenName)?.Value,
                LastName = context.HttpContext?.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Surname)?.Value,
                Email = context.HttpContext?.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value
            };
        }

        public async Task<User?> GetUserData(string id)
        {
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var response = await client.GetAsync(
                $"{credentials.Authority}admin/realms/{credentials.Realm}/users/{id}");

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();

            if (response.StatusCode == HttpStatusCode.Forbidden)
                throw new ForbiddenAccessException();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error: {response.StatusCode}");

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<User>(
                result);
        }

        public async Task<string> AddUser(User user)
        {
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var json = JsonConvert.SerializeObject(new
            {
                id = user.Id,
                username = user.Username,
                firstName = user.FirstName,
                lastName = user.LastName,
                enabled = user.Enabled,
                email = user.Email,
                credentials = new []
                {
                    new {
                            type = "password",
                            value = user.Username,
                            temporary = true
                        }
                },
                requiredActions = user.RequiredActions,
                attributes = user.Attributes
            });
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(
                $"{credentials.Authority}admin/realms/{credentials.Realm}/users/", data);

            if (response.IsSuccessStatusCode)
            {
                var locationHeader = response.Headers.GetValues("Location").FirstOrDefault();
                var userId = locationHeader?.Split('/').Last();

                //if(string.IsNullOrEmpty(user.Password))
                //    await GrantUserChangePasswordAsync(userId);

                return userId;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();

            if (response.StatusCode == HttpStatusCode.Forbidden)
                throw new ForbiddenAccessException();

            if (response.StatusCode == HttpStatusCode.Conflict)
                throw new ConflictException();

            throw new Exception($"StatusCode: {response.StatusCode}");
        }

        public async Task<bool> UpdateUser(string u, User user)
        {
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var json = JsonConvert.SerializeObject(new 
            { 
                id=user.Id,
                username=user.Username,
                firstName=user.FirstName,
                lastName=user.LastName,
                enabled=user.Enabled,
                email =user.Email,
                attributes=user.Attributes
            });
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(
                $"{credentials.Authority}admin/realms/{credentials.Realm}/users/{u}", data);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();

            if (response.StatusCode == HttpStatusCode.Forbidden)
                throw new ForbiddenAccessException();

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveUser(string userId)
        {
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var response = await client.DeleteAsync(
                $"{credentials.Authority}admin/realms/{credentials.Realm}/users/{userId}");

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();

            if (response.StatusCode == HttpStatusCode.Forbidden)
                throw new ForbiddenAccessException();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error: {response.StatusCode}");

            return true;
        }

        public string? GetUserName()
        {
            return context.HttpContext?.User.Claims.First(claim => claim.Type == "preferred_username").Value;
        }

        public string? GetUserId()
        {
            return context.HttpContext?.User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
        }

        public string? GetUserFullNames()
        {
            var givenName = context.HttpContext?.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.GivenName)?.Value;
            var surName = context.HttpContext?.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Surname)?.Value;

            return givenName + (surName is null ? string.Empty : " " + surName);
        }

        public List<string> GetUserRoles()
        {
            var result = context.HttpContext?.User.Claims
                .Where(x => x.Type == ClaimTypes.Role)
                .Select(x => x.Value)
                .ToList();

            return result ??= [];
        }

        public async Task<bool> GrantUserChangePasswordAsync(string userId)
        {
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var response = await client.GetAsync(
                $"{credentials.Authority}admin/realms/{credentials.Realm}/clients?clientId=account");

            if(response.IsSuccessStatusCode)
            {
                var clients = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(
                    await response.Content.ReadAsStringAsync());

                var clientId = clients?[0]["id"].ToString();

                response = await client.GetAsync(
                    $"{credentials.Authority}admin/realms/{credentials.Realm}/clients/{clientId}/roles/manage-account");

                if (response.IsSuccessStatusCode)
                {
                    var role = JsonConvert.DeserializeObject<Dictionary<string, object>>(
                        await response.Content.ReadAsStringAsync());

                    var roleId = role?["id"].ToString();

                    var json = JsonConvert.SerializeObject(new[]
                    {
                        new
                        {
                            id = roleId,
                            name = "manage-account"
                        } 
                    });
                    var data = new StringContent(json, Encoding.UTF8, "application/json");

                    response = await client.PostAsync(
                        $"{credentials.Authority}admin/realms/{credentials.Realm}/users/{userId}/role-mappings/clients/{clientId}", data);

                    if (response.IsSuccessStatusCode)
                        return response.IsSuccessStatusCode;
                }
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();

            if (response.StatusCode == HttpStatusCode.Forbidden)
                throw new ForbiddenAccessException();

            throw new Exception($"Error ensuring access to password change: {response.StatusCode}");
        }
    }
}
