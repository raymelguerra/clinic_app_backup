using ClinicApp.Infrastructure.Dto;
using ClinicApp.WebApp.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace ClinicApp.WebApp.Components;

public partial class MonthlyAbsenteeReport : ComponentBase
{
    [Inject] IReport ReportService { get; set; } = null!;

    #region Montlhy Absentee Report
    private IEnumerable<MontlhyAbsenteeReportDto> _monthlyAttendanceList = [];
    private int _selectedMonth = DateTime.Now.Month;
    private bool OnlyAbsentee = false;
    #endregion

    protected override async Task OnInitializedAsync()
    {
        await LoadReports();
    }

    private async Task LoadReports()
    {
        if (OnlyAbsentee)
        {
            var result = await ReportService.GetMonthlyAbsenteeReportAsync(_selectedMonth);
            _monthlyAttendanceList = result.Where(x => x.AttendanceCount == 0);
        }
        else
            _monthlyAttendanceList = await ReportService.GetMonthlyAbsenteeReportAsync(_selectedMonth);
    }

    private async Task OnMonthChange(string e)
    {
        _selectedMonth = DateTime.ParseExact(e, "MMMM", CultureInfo.InvariantCulture).Month;
        await LoadReports();
    }

    private async Task OnOnlyAbsenteeChange(bool e)
    {
        OnlyAbsentee = e;
        await LoadReports();
    }

    private static string? GetMonthName(int month)
    {
        return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
    }

    private async Task DownloadReport()
    {
        await ReportService.GetMonthlyAbsenteeReportDownloadAsync(_monthlyAttendanceList, NavigationManager);
    }
}
