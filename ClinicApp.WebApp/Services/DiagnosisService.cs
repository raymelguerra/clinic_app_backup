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
    public class DiagnosisService(
        IOptions<ApiSettings> options,
        IUserManagementService userIdpManagement,
        IHttpClientFactory factory,
        NavigationManager navigationManager
        ) : HttpClientServiceBase(factory, navigationManager, userIdpManagement), IDisposable, IDiagnosis
    {
        private readonly ApiSettings apiSettings = options.Value;


        public async Task<IEnumerable<Diagnosis>> GetDiagnosisAsync(string filter)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get, $"{apiSettings.Endpoint}/Diagnoses{filter}");

            using var response = await SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"{response.StatusCode}");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Diagnosis>>(
                result) ?? Array.Empty<Diagnosis>();
        }

        public async Task<Diagnosis?> GetDiagnosisAsync(int id)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get, $"{apiSettings.Endpoint}/Diagnoses/{id}");
            using var response = await SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"{response.StatusCode}");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Diagnosis>(
                result)!;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<bool> PutDiagnosisAsync(int id, Diagnosis Diagnosis)
        {
            var json = JsonConvert.SerializeObject(Diagnosis);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Put, $"{apiSettings.Endpoint}/Diagnoses/{id}")
            {
                Content = content
            };

            var response = await SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> PostDiagnosisAsync(Diagnosis Diagnosis)
        {
            var json = JsonConvert.SerializeObject(Diagnosis);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, $"{apiSettings.Endpoint}/Diagnoses")
            {
                Content = content
            };

            var response = await SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteDiagnosisAsync(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{apiSettings.Endpoint}/Diagnoses/{id}");
            var response = await SendAsync(request);

            return response.IsSuccessStatusCode;
        }
    }
}
