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
    public class ReleaseInformationService(
        IOptions<ApiSettings> options,
        // IUserManagementService userIdpManagement,
        IHttpClientFactory factory,
        NavigationManager navigationManager
        ) : HttpClientServiceBase(factory, navigationManager), IDisposable, IReleaseInformation
    {
        private readonly ApiSettings apiSettings = options.Value;


        public async Task<IEnumerable<ReleaseInformation>> GetReleaseInformationAsync(string filter)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get, $"{apiSettings.Endpoint}/ReleaseInformations{filter}");

            using var response = await SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"{response.StatusCode}");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<ReleaseInformation>>(
                result) ?? Array.Empty<ReleaseInformation>();
        }

        public async Task<ReleaseInformation?> GetReleaseInformationAsync(int id)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get, $"{apiSettings.Endpoint}/ReleaseInformations/{id}");
            using var response = await SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"{response.StatusCode}");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ReleaseInformation>(
                result)!;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
