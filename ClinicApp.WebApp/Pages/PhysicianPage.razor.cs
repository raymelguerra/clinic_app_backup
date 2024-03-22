using ClinicApp.Core.Models;
using ClinicApp.WebApp.Components.Dialogs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ClinicApp.WebApp.Pages;

public partial class PhysicianPage : ComponentBase
{
    [Inject] IDialogService DialogService { get; set; }

    IEnumerable<Contractor> Contractors = new List<Contractor>();

    protected override async Task OnInitializedAsync()
    {
        Contractors = new List<Contractor> {
        new Contractor{
         Extra= "hola bebe"
        }
       };
    }
    private async Task AddPhysician()
    {
        var ctr = new Contractor();

        ctr.Payrolls = new List<Payroll>() {
            new Payroll
            {
                Company = new Company
                {
                    Id = 1,
                    Name = "Expanding Posibilities"
                },
                CompanyId = 1,
                ContractorType = new ContractorType
                {
                    Id = 1,
                    Name ="Analyst"
                },
                ContractorId = 1,
                Procedure = new Procedure(){
                    Id = 1,
                    Name = "2014"
                },
                ProcedureId = 1,

            }
        };
        var parameters = new DialogParameters<PhysicianDialog> { { x => x.Model, ctr } };

        var result = await DialogService.ShowAsync<PhysicianDialog>("Add Physician", parameters);
    }
}
