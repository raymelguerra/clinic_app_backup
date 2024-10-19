using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Data;
using ClinicApp.Infrastructure.Dto.Application;
using ClinicApp.WebApp.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Oauth2.sdk;
using System.Text;

namespace ClinicApp.WebApp.Services
{
    public class ServiceLogService(
        IOptions<ApiSettings> options,
        IUserManagementService userIdpManagement,
        IHttpClientFactory factory,
        NavigationManager navigationManager
        ) : HttpClientServiceBase(factory, navigationManager, userIdpManagement), IDisposable, IServiceLog
    {
        private readonly ApiSettings apiSettings = options.Value;

        public async Task<bool> DeleteServiceLogAsync(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{apiSettings.Endpoint}/ServiceLogs/{id}");
            var response = await SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<ServiceLog>> GetServiceLogAsync(string filter)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get, $"{apiSettings.Endpoint}/ServiceLogs?{filter}");

            using var response = await SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"{response.StatusCode}");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<ServiceLog>>(
                result) ?? Array.Empty<ServiceLog>();
        }

        public async Task<ServiceLog?> GetServiceLogAsync(int id)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get, $"{apiSettings.Endpoint}/ServiceLogs/{id}");
            using var response = await SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"{response.StatusCode}");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ServiceLog>(
                result)!;
        }

        public async Task<bool> PostServiceLogAsync(ServiceLog ServiceLog)
        {
            var json = JsonConvert.SerializeObject(ServiceLog);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, $"{apiSettings.Endpoint}/ServiceLogs")
            {
                Content = content
            };

            var response = await SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> PutServiceLogAsync(int id, ServiceLog ServiceLog)
        {
            var json = JsonConvert.SerializeObject(ServiceLog);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Put, $"{apiSettings.Endpoint}/ServiceLogs/{id}")
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

        public async Task<IEnumerable<ServiceLog>> GetAllServiceLogsByClientContractorPeriodInsurance(int clientId, int contractorId, int periodId, int insuranceId)
        {
            var request = new HttpRequestMessage(
                 HttpMethod.Get, $"{apiSettings.Endpoint}/ServiceLogs/client/{clientId}/contractor/{contractorId}/period/{periodId}/insurance/{insuranceId}");

            using var response = await SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"{response.StatusCode}");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<ServiceLog>>(
                result) ?? Array.Empty<ServiceLog>();
        }

        public async Task<PeriodCalculationResultDto> CalculatePeriodAsync(int periodId)
        {
            var request = new HttpRequestMessage(
                 HttpMethod.Get, $"{apiSettings.Endpoint}/ServiceLogs/billing-profit/{periodId}");
            using var response = await SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"{response.StatusCode}");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PeriodCalculationResultDto>(
                result)!;
        }
    }
}
