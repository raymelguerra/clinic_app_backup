using ClinicApp.Core.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ClinicApp.WebApp.Pages;

public partial class PeriodPaymentDetailPage : ComponentBase
{
    [Inject] IDialogService DialogService { get; set; }
    [Parameter] public int PeriodId { get; set; }

    private Period Period = new Period();

    protected override async Task OnInitializedAsync()
    {
        var Periods = new List<Period> {
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

        Period = Periods.First(x => x.Id == PeriodId);
    }
    private async Task ShowPeriodDetails()
    {

    }

    #region Contractor Search
    private async Task<IEnumerable<string>> SearchContractor(string value)
    {
        // In real life use an asynchronous function for fetching data from an api.
        await Task.Delay(5);

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return states;
        return states.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }
    private string[] states =
  {
        "María García",
    "Juan Pérez",
    "Ana Martínez",
    "Carlos López",
    "Laura Rodríguez",
    "David González",
    "Isabel Fernández",
    "Alejandro Ramírez",
    "Patricia Ruiz",
    "Sergio Herrera",
    "Claudia Medina",
    "Ricardo Torres",
    "Natalia Gómez",
    "Andrés Vargas",
    "Paula Sánchez"
    };

    #endregion
}
