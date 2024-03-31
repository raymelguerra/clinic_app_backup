using ClinicApp.Core.Models;
using ClinicApp.WebApp.Components.Dialogs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ClinicApp.WebApp.Pages;

public partial class ServiceLogPage : ComponentBase
{
    [Inject] IDialogService DialogService { get; set; }

    IEnumerable<ServiceLog> ServiceLogs = new List<ServiceLog>();

    protected override async Task OnInitializedAsync()
    {
        ServiceLogs = new List<ServiceLog> {
        new ServiceLog {
            CreatedDate = DateTime.Now,
            Period =new Period{
                PayPeriod = "PP21"
            },
            Client = new Client{
                Id = 1,
                Name = "Pepe"
            },
            Contractor = new Contractor
            {
                Id = 1,
                Name = "Juana"
            },
            UnitDetails = new List<UnitDetail> {
                new UnitDetail
                {
                    DateOfService = DateTime.Now,
                    Id = 1,
                    PlaceOfService = new PlaceOfService
                    {
                        Name ="Home"
                    },
                    Unit = 18,
                    Procedure = new Procedure
                    {
                        Name ="H2014"
                    }
                }
            }
        }
       };
    }
    private async Task AddServiceLog()
    {
        var sl = new ServiceLog();

        sl.UnitDetails.Add(new UnitDetail
        {
            DateOfService = DateTime.Now,
            Id = 1,
            PlaceOfService = new PlaceOfService
            {
                Name = "Home"
            },
            Unit = 18,
            Procedure = new Procedure
            {
                Name = "H2014"
            }
        });

        var parameters = new DialogParameters<ServiceLogDialog> { { x => x.Model, sl } };

        var result = await DialogService.ShowAsync<ServiceLogDialog>("Add Service Log", parameters);
    }
}
