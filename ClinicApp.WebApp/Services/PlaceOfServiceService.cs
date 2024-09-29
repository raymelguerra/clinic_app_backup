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
    public class PlaceOfServiceService(
        IOptions<ApiSettings> options,
        IUserManagementService userIdpManagement,
        IHttpClientFactory factory,
        NavigationManager navigationManager
        ) : HttpClientServiceBase(factory, navigationManager, userIdpManagement), IDisposable, IPlaceOfService
    {
        private readonly ApiSettings apiSettings = options.Value;

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<IEnumerable<PlaceOfService>> GetPlaceOfServiceAsync(string filter)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get, $"{apiSettings.Endpoint}/PlaceOfServices{filter}");

            using var response = await SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"{response.StatusCode}");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<PlaceOfService>>(
                result) ?? Array.Empty<PlaceOfService>();
        }

        public Task<PlaceOfService?> GetPlaceOfServiceAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> PutPlaceOfServiceAsync(int id, PlaceOfService PlaceOfService)
        {
            throw new NotImplementedException();
        }

        public Task<bool> PostPlaceOfServiceAsync(PlaceOfService PlaceOfService)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeletePlaceOfServiceAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
