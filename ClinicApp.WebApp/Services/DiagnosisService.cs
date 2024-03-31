using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Data;
using ClinicApp.WebApp.Interfaces;
using Ipcs.WebApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace ClinicApp.WebApp.Services
{
    public class DiagnosisService(
        IOptions<ApiSettings> options,
        // IUserManagementService userIdpManagement,
        IHttpClientFactory factory,
        NavigationManager navigationManager
        ) : HttpClientServiceBase(factory, navigationManager), IDisposable, IDiagnosis
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
    }
}
