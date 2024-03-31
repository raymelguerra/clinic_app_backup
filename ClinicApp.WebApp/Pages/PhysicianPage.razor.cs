using ClinicApp.Core.Models;
using ClinicApp.WebApp.Components.Dialogs;
using ClinicApp.WebApp.Interfaces;
using ClinicApp.WebApp.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ClinicApp.WebApp.Pages;

public partial class PhysicianPage : ComponentBase
{
    [Inject] IDialogService DialogService { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }
    [Inject] IPhysician PhysicianService { get; set; }
    public bool _loading = false;

    IEnumerable<Contractor> Contractors = new List<Contractor>();

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _loading = true;
            Contractors = await PhysicianService.GetPhysicianAsync("");
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Oops, an error occurred. The error type is: {ex.Message}.", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }
    private async Task AddPhysician()
    {
        Contractor ctr = new();
        ctr.Payrolls = new();

        var parameters = new DialogParameters<PhysicianDialog> { { x => x.Model, ctr } };

        var result = await DialogService.ShowAsync<PhysicianDialog>("Add Physician", parameters);
    }
}
