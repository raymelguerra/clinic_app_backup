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
    public class ContractorTypeService(
        IOptions<ApiSettings> options,
        // IUserManagementService userIdpManagement,
        IHttpClientFactory factory,
        NavigationManager navigationManager
        ) : HttpClientServiceBase(factory, navigationManager), IDisposable, IContractorType
    {
        private readonly ApiSettings apiSettings = options.Value;


        public async Task<IEnumerable<ContractorType>> GetContractorTypeAsync(string filter)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get, $"{apiSettings.Endpoint}/ContractorTypes{filter}");

            using var response = await SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"{response.StatusCode}");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<ContractorType>>(
                result) ?? Array.Empty<ContractorType>();
        }

        public async Task<ContractorType?> GetContractorTypeAsync(int id)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get, $"{apiSettings.Endpoint}/ContractorTypes/{id}");
            using var response = await SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"{response.StatusCode}");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ContractorType>(
                result)!;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
