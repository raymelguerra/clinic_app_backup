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
        
        Period = Periods.First(x=> x.Id == PeriodId);
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
        "Alabama", "Alaska", "American Samoa", "Arizona",
        "Arkansas", "California", "Colorado", "Connecticut",
        "Delaware", "District of Columbia", "Federated States of Micronesia",
        "Florida", "Georgia", "Guam", "Hawaii", "Idaho",
        "Illinois", "Indiana", "Iowa", "Kansas", "Kentucky",
        "Louisiana", "Maine", "Marshall Islands", "Maryland",
        "Massachusetts", "Michigan", "Minnesota", "Mississippi",
        "Missouri", "Montana", "Nebraska", "Nevada",
        "New Hampshire", "New Jersey", "New Mexico", "New York",
        "North Carolina", "North Dakota", "Northern Mariana Islands", "Ohio",
        "Oklahoma", "Oregon", "Palau", "Pennsylvania", "Puerto Rico",
        "Rhode Island", "South Carolina", "South Dakota", "Tennessee",
        "Texas", "Utah", "Vermont", "Virgin Island", "Virginia",
        "Washington", "West Virginia", "Wisconsin", "Wyoming",
    };

    #endregion
}
