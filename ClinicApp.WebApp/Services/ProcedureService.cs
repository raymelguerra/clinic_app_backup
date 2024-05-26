using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Data;
using ClinicApp.WebApp.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Oauth2.sdk;
using System.Text;

namespace ClinicApp.WebApp.Services
{
    public class ProcedureService(
        IOptions<ApiSettings> options,
        IUserManagementService userIdpManagement,
        IHttpClientFactory factory,
        NavigationManager navigationManager
        ) : HttpClientServiceBase(factory, navigationManager, userIdpManagement), IDisposable, IProcedure
    {
        private readonly ApiSettings apiSettings = options.Value;

        public async Task<bool> DeleteProcedureAsync(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{apiSettings.Endpoint}/Procedures/{id}");
            var response = await SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<Procedure>> GetProcedureAsync(string filter)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get, $"{apiSettings.Endpoint}/Procedures{filter}");

            using var response = await SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"{response.StatusCode}");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Procedure>>(
                result) ?? Array.Empty<Procedure>();
        }

        public async Task<Procedure?> GetProcedureAsync(int id)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get, $"{apiSettings.Endpoint}/Procedures/{id}");
            using var response = await SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"{response.StatusCode}");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Procedure>(
                result)!;
        }

        public async Task<bool> PostProcedureAsync(Procedure procedure)
        {
            var json = JsonConvert.SerializeObject(procedure);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, $"{apiSettings.Endpoint}/Procedures")
            {
                Content = content
            };

            var response = await SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> PutProcedureAsync(int id, Procedure procedure)
        {
            var json = JsonConvert.SerializeObject(procedure);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Put, $"{apiSettings.Endpoint}/Procedures/{id}")
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
