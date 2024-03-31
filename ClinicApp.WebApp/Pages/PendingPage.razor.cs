using ClinicApp.Core.Models;
using ClinicApp.WebApp.Components.Dialogs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ClinicApp.WebApp.Pages;

public partial class PendingPage : ComponentBase
{
    [Inject] IDialogService DialogService { get; set; }

    IEnumerable<ServiceLog> ServiceLogs = new List<ServiceLog>();

    protected override async Task OnInitializedAsync()
    {
        ServiceLogs = new List<ServiceLog> {
        new ServiceLog {
            Pending = "Pending by: Patient account number invalid",
            CreatedDate = DateTime.Now,
            Period =new Period{
                PayPeriod = "PP21"
            },
            Client = new Client{
                Id = 1,
                Name = "Pepe Hernandez Contreras"
            },
            ClientId = 1,
            Contractor = new Contractor
            {
                Id = 1,
                Name = "Juana Perez Perez"
            },
            ContractorId = 1,
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
    private async Task ShowDialog(int Id, PendingBy type)
    {
        if (type == PendingBy.PHYSICIAN)
        {
            var data = ServiceLogs.First(x => x.ContractorId == Id);
            if (data != null)
            {
                var parameters = new DialogParameters<PhysicianDialog> { { x => x.Model, data.Contractor } };
                var result = await DialogService.ShowAsync<PhysicianDialog>("Edit Physician", parameters);
            }
        }
        else
        {
            var data = ServiceLogs.First(x => x.ClientId == Id);
            if (data != null)
            {
                var parameters = new DialogParameters<PatientDialog> { { x => x.Model, data.Client } };
                var result = await DialogService.ShowAsync<PatientDialog>("Edit Patient", parameters);
            }
        }
    }
    private enum PendingBy
    {
        PHYSICIAN, PATIENT
    }
}
