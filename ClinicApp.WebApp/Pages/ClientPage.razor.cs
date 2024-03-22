using ClinicApp.Core.Models;
using ClinicApp.WebApp.Components.Dialogs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ClinicApp.WebApp.Pages;

public partial class ClientPage : ComponentBase
{
    [Inject] IDialogService DialogService { get; set; }

    IEnumerable<Client> Clients = new List<Client>();

    protected override async Task OnInitializedAsync()
    {
        Clients = new List<Client> {
        new Client{
         Name= "hola bebe"
        }
       };
    }
    private async Task AddClient()
    {
        var cl = new Client();

        cl.Agreements = new List<Agreement>() {
            new Agreement
            {
                Company = new Company
                {
                    Id = 1,
                    Name = "Expanding Possibilities"
                },
                CompanyId = 1,
                Payroll = new Payroll
                {
                    Contractor = new Contractor
                    {
                        Name= "Juan Perez"
                    },
                    ContractorId = 1,
                    ContractorType = new ContractorType
                    {
                        Name = "Analyst"
                    },
                    Procedure = new Procedure
                    {
                        Name = "2014"
                    }
                },
                RateEmployees = 50
            }
        };
        var parameters = new DialogParameters<PatientDialog> { { x => x.Model, cl } };

        var result = await DialogService.ShowAsync<PatientDialog>("Add Patient", parameters);
    }
}
