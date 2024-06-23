using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Oauth2.sdk.Exceptions;
using Oauth2.sdk.Models;
using System;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Oauth2.sdk.Services.KeycloakProvider
{
    public class RolesManagementService_KeycloakProvider : KeycloakProviderBase, IRolesManagementService
    {
        public RolesManagementService_KeycloakProvider(
            IOptions<CredentialsSettings> _options,
            IHttpContextAccessor _httpContextAccessor
        ) : base(_options, _httpContextAccessor) { }

        public async Task<IEnumerable<Role>?> GetRolesByUser(string userId)
        {
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var clientUuid = await GetIdClient();
            var response = await client.GetAsync(
                $"{credentials.Authority}admin/realms/{credentials.Realm}/users/{userId}/role-mappings/clients/{clientUuid}");

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();

            if (response.StatusCode == HttpStatusCode.Forbidden)
                throw new ForbiddenAccessException();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error: {response.StatusCode}");

            return JsonConvert.DeserializeObject<IEnumerable<Role>>(
                await response.Content.ReadAsStringAsync());
        }

        public async Task<Role?> GetRoleData(string id)
        {
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var response = await client.GetAsync(
                $"{credentials.Authority}admin/realms/{credentials.Realm}/roles/{id}");

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();

            if (response.StatusCode == HttpStatusCode.Forbidden)
                throw new ForbiddenAccessException();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error: {response.StatusCode}");

            return JsonConvert.DeserializeObject<Role>(
                await response.Content.ReadAsStringAsync());
        }

        public async Task<bool> AddRole(Role role)
        {
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var json = JsonConvert.SerializeObject(role);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(
                $"{credentials.Authority}admin/realms/{credentials.Realm}/roles/", data);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateRole(Role role)
        {
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var json = JsonConvert.SerializeObject(role);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync(
                $"{credentials.Authority}admin/realms/{credentials.Realm}/roles/{role.Id}", data);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();

            if (response.StatusCode == HttpStatusCode.Forbidden)
                throw new ForbiddenAccessException();

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveRole(string roleId)
        {
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var response = await client.DeleteAsync(
                $"{credentials.Authority}admin/realms/{credentials.Realm}/roles/{roleId}");

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();

            if (response.StatusCode == HttpStatusCode.Forbidden)
                throw new ForbiddenAccessException();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error: {response.StatusCode}");

            return true;
        }

        public async Task<IEnumerable<Role>?> GetListClientRoles(string clientId = "")
        {
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var response = await client.GetAsync(
                $"{credentials.Authority}admin/realms/{credentials.Realm}/clients/{await GetIdClient(clientId)}/roles");

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();

            if (response.StatusCode == HttpStatusCode.Forbidden)
                throw new ForbiddenAccessException();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error: {response.StatusCode}");

            return JsonConvert.DeserializeObject<IEnumerable<Role>>(
                await response.Content.ReadAsStringAsync());
        }

        public List<string> GetUserRoles()
        {
            var result = context.HttpContext?.User.Claims
                .Where(x => x.Type == ClaimTypes.Role)
                .Select(x => x.Value)
                .ToList();

            return result ??= new List<string>();
        }

        public bool IsInRole(string role)
        {
            var roleList = context.HttpContext?.User.Claims
                .Where(x => x.Type == ClaimTypes.Role)
                .Select(x => x.Value)
                .ToList();

            if (roleList is not null)
                return roleList!.Contains(role);

            return false;
        }

        public async Task<bool> AddRoleToUser(string userId, IEnumerable<Role> roles)
        {
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var clientUuid = await GetIdClient();
            bool[] results = new bool[roles.Count()];

            foreach (var role in roles)
            {
                var response = await client.PostAsync(
                                   $"{credentials.Authority}admin/realms/{credentials.Realm}/users/{userId}/role-mappings/clients/{clientUuid}",
                                                  new StringContent($"[{{\"id\":\"{role.Id}\",\"name\":\"{role.Name}\"}}]", Encoding.UTF8, "application/json"));

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedAccessException();

                if (response.StatusCode == HttpStatusCode.Forbidden)
                    throw new ForbiddenAccessException();

                results[roles.ToList().IndexOf(role)] = response.IsSuccessStatusCode;
            }
            return results.All(x => x);
        }

        public async Task<bool> RemoveRoleFromUser(string userId)
        {
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var clientUuid = await GetIdClient();

            var response = await client.DeleteAsync(
                                   $"{credentials.Authority}admin/realms/{credentials.Realm}/users/{userId}/role-mappings/clients/{clientUuid}");

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();

            if (response.StatusCode == HttpStatusCode.Forbidden)
                throw new ForbiddenAccessException();

            return response.IsSuccessStatusCode;
        }
    }
}
