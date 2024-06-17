
using ClinicApp.Infrastructure.Dto;
using Microsoft.AspNetCore.Components;

namespace ClinicApp.WebApp.Interfaces;

public interface IReport
{
    public Task<IEnumerable<MontlhyAbsenteeReportDto>> GetMonthlyAbsenteeReportAsync(int month);
    public Task GetMonthlyAbsenteeReportDownloadAsync(IEnumerable<MontlhyAbsenteeReportDto> data, NavigationManager NavigationManager);
}
