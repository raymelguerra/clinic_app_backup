using ClinicApp.Infrastructure.Data;
using ClinicApp.Infrastructure.Dto;
using ClinicApp.WebApp.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Oauth2.sdk;
using System.Text;

namespace ClinicApp.WebApp.Services
{
    public class ReportService(
        IOptions<ApiSettings> options,
        IUserManagementService userIdpManagement,
        IHttpClientFactory factory,
        NavigationManager navigationManager,
        IJSRuntime jsRuntime
        ) : HttpClientServiceBase(factory, navigationManager, userIdpManagement), IReport
    {
        private readonly ApiSettings apiSettings = options.Value;
        private readonly IJSRuntime _jsRuntime = jsRuntime;

        public async Task<IEnumerable<MontlhyAbsenteeReportDto>> GetMonthlyAbsenteeReportAsync(int month)
        {
            var request = new HttpRequestMessage(
                    HttpMethod.Get, $"{apiSettings.Endpoint}/Reports/montlhy-absentee-report?month={month}");

            using var response = await SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"{response.StatusCode}");

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<MontlhyAbsenteeReportDto>>(
                result) ?? [];
        }

        public async Task GetMonthlyAbsenteeReportDownloadAsync(IEnumerable<MontlhyAbsenteeReportDto> data, NavigationManager NavigationManager)
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, $"{apiSettings.Endpoint}/Reports/montlhy-absentee-report/download")
            {
                Content = content
            };

            using var response = await SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var pdfBytes = await response.Content.ReadAsByteArrayAsync();
                var base64 = Convert.ToBase64String(pdfBytes);

                var url = $"data:application/pdf;base64,{base64}";

                Console.WriteLine(url);

                await _jsRuntime.InvokeVoidAsync("downloadFile", base64, "report.pdf", "application/pdf");
            }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
