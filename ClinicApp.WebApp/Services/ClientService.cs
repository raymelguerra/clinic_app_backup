using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Data;
using ClinicApp.WebApp.Interfaces;
using Ipcs.WebApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Oauth2.sdk;
using System.Text;

namespace ClinicApp.WebApp.Services
{
    public class ClientService(
        IOptions<ApiSettings> options,
        IUserManagementService userIdpManagement,
        IHttpClientFactory factory,
        NavigationManager navigationManager
       ) : HttpClientServiceBase(factory, navigationManager, userIdpManagement), IDisposable, IClient
    {
        private readonly ApiSettings apiSettings = options.Value;

        public async Task<bool> DeleteClientAsync(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{apiSettings.Endpoint}/Clients/{id}");
            var response = await SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<Client>> GetClientAsync(string filter)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get, $"{apiSettings.Endpoint}/Clients{filter}");

            using var response = await SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"{response.StatusCode}");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Client>>(
                result) ?? Array.Empty<Client>();
        }

        public async Task<Client?> GetClientAsync(int id)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get, $"{apiSettings.Endpoint}/Clients/{id}");
            using var response = await SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"{response.StatusCode}");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Client>(
                result)!;
        }

        public async Task<bool> PostClientAsync(Client client)
        {
            var json = JsonConvert.SerializeObject(client);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, $"{apiSettings.Endpoint}/Clients")
            {
                Content = content
            };

            var response = await SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> PutClientAsync(int id, Client client)
        {
            var json = JsonConvert.SerializeObject(client);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Put, $"{apiSettings.Endpoint}/Clients/{id}")
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
