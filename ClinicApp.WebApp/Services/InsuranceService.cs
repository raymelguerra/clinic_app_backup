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
    public class InsuranceService(
        IOptions<ApiSettings> options,
        IUserManagementService userIdpManagement,
        IHttpClientFactory factory,
        NavigationManager navigationManager
        ) : HttpClientServiceBase(factory, navigationManager, userIdpManagement), IDisposable, IInsurance
    {
        private readonly ApiSettings apiSettings = options.Value;

        public async Task<bool> DeleteInsuranceAsync(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{apiSettings.Endpoint}/Insurances/{id}");
            var response = await SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<Insurance>> GetInsuranceAsync(string filter)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get, $"{apiSettings.Endpoint}/Insurances{filter}");

            using var response = await SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"{response.StatusCode}");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Insurance>>(
                result) ?? Array.Empty<Insurance>();
        }

        public async Task<Insurance?> GetInsuranceAsync(int id)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get, $"{apiSettings.Endpoint}/Insurances/{id}");
            using var response = await SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"{response.StatusCode}");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Insurance>(
                result)!;
        }

        public async Task<bool> PostInsuranceAsync(Insurance insurance)
        {
            var json = JsonConvert.SerializeObject(insurance);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, $"{apiSettings.Endpoint}/Insurances")
            {
                Content = content
            };

            var response = await SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> PutInsuranceAsync(int id, Insurance insurance)
        {
            var json = JsonConvert.SerializeObject(insurance);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Put, $"{apiSettings.Endpoint}/Insurances/{id}")
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
