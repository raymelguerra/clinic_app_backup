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
    public class PayrollService(
        IOptions<ApiSettings> options,
        IUserManagementService userIdpManagement,
        IHttpClientFactory factory,
        NavigationManager navigationManager
        ) : HttpClientServiceBase(factory, navigationManager, userIdpManagement), IDisposable, IPayroll
    {
        private readonly ApiSettings apiSettings = options.Value;

        public Task<bool> DeletePayrollAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<IEnumerable<Payroll>> GetPayrollAsync(string filter)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get, $"{apiSettings.Endpoint}/Payroll{filter}");

            using var response = await SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"{response.StatusCode}");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Payroll>>(
                result) ?? [];
        }

        public Task<Payroll?> GetPayrollAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Payroll>> GetPayrollsByInsuranceAsync(int insuranceId)
        {
            var request = new HttpRequestMessage(
                               HttpMethod.Get, $"{apiSettings.Endpoint}/Payroll/insurance/{insuranceId}");
            using var response = await SendAsync(request);
            
            if (!response.IsSuccessStatusCode)
                throw new Exception($"{response.StatusCode}");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Payroll>>(
                result) ?? [];
        }

        public Task<bool> PostPayrollAsync(Payroll Payroll)
        {
            throw new NotImplementedException();
        }

        public Task<bool> PutPayrollAsync(int id, Payroll Payroll)
        {
            throw new NotImplementedException();
        }
    }
}
