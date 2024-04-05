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

        var dialog = await DialogService.ShowAsync<PhysicianDialog>("Add Physician", parameters);
        var result = await dialog.Result;
        if (!result.Canceled && (bool)result.Data)
        {
            Snackbar.Add($"Physician successfully created", Severity.Success);
            await OnInitializedAsync();
        }
    }

    private async Task EditClient(int physicianId)
    {
        var ph = await PhysicianService.GetPhysicianAsync(physicianId);
        if (ph != null)
        {
            ph.Payrolls = ph.Payrolls ?? new();

            var parameters = new DialogParameters<PhysicianDialog> { { x => x.Model, ph } };

            var dialog = await DialogService.ShowAsync<PhysicianDialog>("Edit Physician", parameters);
            var result = await dialog.Result;
            if (!result.Canceled && (bool)result.Data)
            {
                Snackbar.Add($"Physician successfully updated", Severity.Success);
                await OnInitializedAsync();
            }
        }
        else
        {
            Snackbar.Add($"Oops! An error has occurred. This patient is not in the database.", Severity.Error);
        }
    }
    private async Task RemovePhysician(int PhysicianId)
    {
        var options = new DialogOptions
        {
            DisableBackdropClick = false,
            MaxWidth = MaxWidth.Small,
            Position = DialogPosition.Center,
        };
        var dialog = await DialogService.ShowAsync<DeleteDialog>("Delete Physician", options);
        var result = await dialog.Result;
        if (!result.Canceled && (bool)result.Data)
        {
            var delete = await PhysicianService.DeletePhysicianAsync(PhysicianId);
            if (delete)
            {
                Snackbar.Add($"Physician successfully deleted", Severity.Success);
                await OnInitializedAsync();
            }
            else
            {
                Snackbar.Add($"Oops! An error has occurred. This physician is not in the database.", Severity.Error);
            }
        }
    }
}
