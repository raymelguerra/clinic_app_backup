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
    public class PhysicianService(
        IOptions<ApiSettings> options,
        IUserManagementService userIdpManagement,
        IHttpClientFactory factory,
        NavigationManager navigationManager
        ) : HttpClientServiceBase(factory, navigationManager, userIdpManagement), IPhysician
    {
        private readonly ApiSettings apiSettings = options.Value;

        public async Task<bool> DeletePhysicianAsync(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{apiSettings.Endpoint}/Contractors/{id}");
            var response = await SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<Contractor>> GetPhysicianAsync(string filter)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get, $"{apiSettings.Endpoint}/Contractors{filter}");

            using var response = await SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"{response.StatusCode}");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Contractor>>(
                result) ?? Array.Empty<Contractor>();
        }

        public async Task<Contractor?> GetPhysicianAsync(int id)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get, $"{apiSettings.Endpoint}/Contractors/{id}");
            using var response = await SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"{response.StatusCode}");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Contractor>(
                result)!;
        }

        public async Task<bool> PostPhysicianAsync(Contractor contractor)
        {
            var json = JsonConvert.SerializeObject(contractor);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, $"{apiSettings.Endpoint}/Contractors")
            {
                Content = content
            };

            var response = await SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> PutPhysicianAsync(int id, Contractor contractor)
        {
            var json = JsonConvert.SerializeObject(contractor);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Put, $"{apiSettings.Endpoint}/Contractors/{id}")
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

        public async Task<IEnumerable<Contractor>> GetContractoresByProcedureAndInsurance(int procedureId, int insuranceId)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get, $"{apiSettings.Endpoint}/procedure/{procedureId}/insurance/{insuranceId}");

            using var response = await SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"{response.StatusCode}");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Contractor>>(
                result) ?? Array.Empty<Contractor>();
        }
    }
}
