using ClinicApp.Core.Models;
using ClinicApp.WebApp.Components.Dialogs;
using ClinicApp.WebApp.Interfaces;
using ClinicApp.WebApp.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ClinicApp.WebApp.Pages;

public partial class PeriodPaymentPage : ComponentBase
{
    [Inject] IDialogService DialogService { get; set; } = null!;
    [Inject] NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IPeriod PeriodService { get; set; } = null!;

    IEnumerable<Period> Periods = new List<Period>();

    protected override async Task OnInitializedAsync()
    {
        Periods = await PeriodService.GetPeriodAsync("");
    }
    private async Task ShowPeriodDetails(int periodId)
    {
        NavigationManager.NavigateTo($"/periodpaymentdetails/{periodId}", true);
    }
}
