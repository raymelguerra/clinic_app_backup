using ClinicApp.Core.Dtos;
using ClinicApp.Infrastructure.Data;
using ClinicApp.WebApp.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Oauth2.sdk;
using Oauth2.sdk.Models;
using System.Text;

namespace ClinicApp.WebApp.Services;

public class SecurityManagementService(
    IOptions<ApiSettings> options,
    IUserManagementService userIdpManagement,
    IHttpClientFactory factory,
    NavigationManager navigationManager
    ) : HttpClientServiceBase(factory, navigationManager, userIdpManagement), IDisposable, ISecurityManagement
{
    private readonly ApiSettings apiSettings = options.Value;

    public async Task<IEnumerable<UserVM>?> GetUsersAsync(string filter = "")
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get, $"{apiSettings.Endpoint}/Users{filter}");

        using var response = await SendAsync(request);

        if (!response.IsSuccessStatusCode)
            throw new Exception($"{response.StatusCode}");

        var result = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<IEnumerable<UserVM>>(result);
    }

    public async Task<UserVM?> GetUserAsync(string id)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get, $"{apiSettings.Endpoint}/Users/{id}");

        using var response = await SendAsync(request);

        if (!response.IsSuccessStatusCode)
            throw new Exception($"{response.StatusCode}");

        var result = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<UserVM>(result);
    }

    public async Task<bool> CreateUserAsync(UserVM user)
    {
        user.Id = string.Empty;
        var json = JsonConvert.SerializeObject(user);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Post, $"{apiSettings.Endpoint}/Users")
        {
            Content = content
        };

        var response = await SendAsync(request);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateUserAsync(UserVM user, string id, bool inDb = false)
    {
        var json = JsonConvert.SerializeObject(user);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Put, $"{apiSettings.Endpoint}/Users/{id ?? user.Username}?updateindb={inDb}")
        {
            Content = content
        };

        var response = await SendAsync(request);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> RemoveUserAsync(string id)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"{apiSettings.Endpoint}/Users/{id}");
        var response = await SendAsync(request);

        return response.IsSuccessStatusCode;
    }

    public async IAsyncEnumerable<UserVM?> GetUsersStream(string filter = "")
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get, $"{apiSettings.Endpoint}/Users/stream{filter}");

        using var responseStream = await SendAsyncGetStream(request);

        await foreach (var user in DeserializeJsonStream<UserVM>(responseStream))
        {
            yield return user;
        }
    }

    public async Task<IEnumerable<string>?> GetUserRolesByUserNameAsync(string username)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get, $"{apiSettings.Endpoint}/Users/{username}/roles");

        using var response = await SendAsync(request);

        var result = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<IEnumerable<string>>(result);
    }

    public async Task<bool> UpdateUserRolesByUserAsync(UserVM user, string username)
    {
        var json = JsonConvert.SerializeObject(user);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Put, $"{apiSettings.Endpoint}/Users/{username}/roles")
        {
            Content = content
        };

        var response = await SendAsync(request);

        return response.IsSuccessStatusCode;
    }

    public async Task<IEnumerable<Role>?> GetRolesAsync(string filter = "")
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get, $"{apiSettings.Endpoint}/Roles{filter}");

        using var response = await SendAsync(request);

        var result = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<IEnumerable<Role>>(result);
    }


    public async Task<bool> CreateRoleAsync(Role role)
    {
        var json = JsonConvert.SerializeObject(role);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Post, $"{apiSettings.Endpoint}/Roles")
        {
            Content = content
        };

        var response = await SendAsync(request);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateRolesAsync(Role role, int roleId)
    {
        var json = JsonConvert.SerializeObject(role);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Put, $"{apiSettings.Endpoint}/Roles/{roleId}")
        {
            Content = content
        };

        var response = await SendAsync(request);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> RemoveRoleAsync(int roleId)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"{apiSettings.Endpoint}/Roles/{roleId}");
        var response = await SendAsync(request);

        return response.IsSuccessStatusCode;
    }

    public async Task<IEnumerable<string>?> GetDepartmentsAsync(string filter = "")
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get, $"{apiSettings.Endpoint}/Departments{filter}");

        using var response = await SendAsync(request);

        var result = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<IEnumerable<string>>(result);
    }

    private static async IAsyncEnumerable<T> DeserializeJsonStream<T>(Stream stream)
    {
        using var reader = new StreamReader(stream);
        string line;

        while ((line = await reader.ReadLineAsync()) != null)
        {
            yield return JsonConvert.DeserializeObject<T>(line)!;
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
