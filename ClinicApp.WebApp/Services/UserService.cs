using ClinicApp.Core.Dtos;
using ClinicApp.Infrastructure.Data;
using ClinicApp.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Oauth2.sdk;
using System.Text;

namespace ClinicApp.WebApp.Services
{
    public class UserService(
        IOptions<ApiSettings> options,
        IUserManagementService userIdpManagement,
        IHttpClientFactory factory,
        NavigationManager navigationManager
        ) : HttpClientServiceBase(factory, navigationManager, userIdpManagement), IDisposable, IUsersService
    {
        private readonly ApiSettings apiSettings = options.Value;

        public async Task<bool> DeleteUser(string userId)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{apiSettings.Endpoint}/Users/{userId}");
            var response = await SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<UserVM>> GetAllUsers()
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get, $"{apiSettings.Endpoint}/Users");

            using var response = await SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"{response.StatusCode}");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<UserVM>>(
                result) ?? Array.Empty<UserVM>();
        }

        public async Task<UserVM?> GetUser(string userId)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get, $"{apiSettings.Endpoint}/Users/{userId}");
            using var response = await SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"{response.StatusCode}");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserVM>(
                result)!;
        }

        public async Task<bool> CreateUser(UserVM user)
        {
            user.Username = user.Email;
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, $"{apiSettings.Endpoint}/Users")
            {
                Content = content
            };

            var response = await SendAsync(request);
            
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateUser(string userId, UserVM user)
        {
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Put, $"{apiSettings.Endpoint}/Users/{userId}")
            {
                Content = content
            };

            var response = await SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
