using ClinicApp.Core.Models;
using ClinicApp.WebApp.Components.Dialogs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ClinicApp.WebApp.Pages;

public partial class PeriodPaymentPage : ComponentBase
{
    [Inject] IDialogService DialogService { get; set; }
    [Inject] NavigationManager NavigationManager { get; set; }

    IEnumerable<Period> Periods = new List<Period>();

    protected override async Task OnInitializedAsync()
    {
        Periods = new List<Period> {
        new Period {
            Active = true,
            DocumentDeliveryDate = DateTime.Now,
            EndDate = DateTime.Now,
            PaymentDate = DateTime.Now,
            Id = 1,
            PayPeriod = "PP01",
            StartDate = DateTime.Now
        },
        new Period {
            Active = true,
            DocumentDeliveryDate = DateTime.Now,
            EndDate = DateTime.Now,
            PaymentDate = DateTime.Now,
            Id = 2,
            PayPeriod = "PP02",
            StartDate = DateTime.Now
        }
       };
    }
    private async Task ShowPeriodDetails(int periodId)
    {
        NavigationManager.NavigateTo($"/periodpaymentdetails/{periodId}", true);
    }
}
