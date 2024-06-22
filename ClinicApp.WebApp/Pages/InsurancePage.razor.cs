using ClinicApp.Core.Models;
using ClinicApp.WebApp.Components.Dialogs;
using ClinicApp.WebApp.Interfaces;
using ClinicApp.WebApp.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ClinicApp.WebApp.Pages;

public partial class InsurancePage : ComponentBase
{
    [Inject] IDialogService DialogService { get; set; } = null!;
    [Inject] ISnackbar Snackbar { get; set; } = null!;
    [Inject] IInsurance InsuranceService { get; set; } = null!;
    public bool _loading = false;

    IEnumerable<Insurance> Insurances = new List<Insurance>();

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _loading = true;
            Insurances = await InsuranceService.GetInsuranceAsync("");
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
    private async Task AddInsurance()
    {
        var ins = new Insurance();
        ins.InsuranceProcedures = new();

        var parameters = new DialogParameters<InsuranceDialog> { { x => x.Model, ins } };
        var dialog = await DialogService.ShowAsync<InsuranceDialog>("Add Insurance", parameters);
        var result = await dialog.Result;
        if (!result.Canceled && (bool)result.Data)
        {
            Snackbar.Add($"Insurance successfully created", Severity.Success);
            await OnInitializedAsync();
        }
    }

    private async Task EditInsurance(int insuranceId)
    {
        var ins = await InsuranceService.GetInsuranceAsync(insuranceId);
        if (ins != null)
        {
            ins.InsuranceProcedures = ins.InsuranceProcedures ?? new();

            var parameters = new DialogParameters<InsuranceDialog> { { x => x.Model, ins } };

            var dialog = await DialogService.ShowAsync<InsuranceDialog>("Edit Insurance", parameters);
            var result = await dialog.Result;
            if (!result.Canceled && (bool)result.Data)
            {
                Snackbar.Add($"Insurance successfully updated", Severity.Success);
                await OnInitializedAsync();
            }
        }
        else
        {
            Snackbar.Add($"Oops! An error has occurred. This insurance is not in the database.", Severity.Error);
        }
    }
    private async Task RemoveInsurance(int insuranceId)
    {
        var options = new DialogOptions
        {
            DisableBackdropClick = false,
            MaxWidth = MaxWidth.Small,
            Position = DialogPosition.Center,
        };
        var dialog = await DialogService.ShowAsync<DeleteDialog>("Delete Insurance", options);
        var result = await dialog.Result;
        if (!result.Canceled && (bool)result.Data)
        {
            var delete = await InsuranceService.DeleteInsuranceAsync(insuranceId);
            if (delete)
            {
                Snackbar.Add($"Insurance successfully deleted", Severity.Success);
                await OnInitializedAsync();
            }
            else
            {
                Snackbar.Add($"Oops! An error has occurred. This Insurance is not in the database.", Severity.Error);
            }
        }
    }
}
