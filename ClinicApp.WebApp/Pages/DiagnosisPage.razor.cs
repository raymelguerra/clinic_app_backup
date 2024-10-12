using ClinicApp.Core.Models;
using ClinicApp.WebApp.Components.Dialogs;
using ClinicApp.WebApp.Interfaces;
using ClinicApp.WebApp.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ClinicApp.WebApp.Pages;

public partial class DiagnosisPage : ComponentBase
{
    [Inject] IDialogService DialogService { get; set; } = null!;
    [Inject] ISnackbar Snackbar { get; set; } = null!;
    [Inject] IDiagnosis DiagnosisService { get; set; } = null!;
    public bool _loading = false;

    IEnumerable<Diagnosis> Diagnoses = new List<Diagnosis>();

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _loading = true;
            Diagnoses = await DiagnosisService.GetDiagnosisAsync("");
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
    private async Task AddDiagnosis()
    {
        var diagnosis = new Diagnosis();

        var parameters = new DialogParameters<DiagnosisDialog> { { x => x.Model, diagnosis } };
        var dialog = await DialogService.ShowAsync<DiagnosisDialog>("Add Diagnosis", parameters);
        var result = await dialog.Result;
        if (!result.Canceled && (bool)result.Data)
        {
            Snackbar.Add($"Diagnosis successfully created", Severity.Success);
            await OnInitializedAsync();
        }
    }

    private async Task EditDiagnosis(int diagnosisId)
    {
        var diagnosis = await DiagnosisService.GetDiagnosisAsync(diagnosisId);
        if (diagnosis != null)
        {
            var parameters = new DialogParameters<DiagnosisDialog> { { x => x.Model, diagnosis } };

            var dialog = await DialogService.ShowAsync<DiagnosisDialog>("Edit Diagnosis", parameters);
            var result = await dialog.Result;
            if (!result.Canceled && (bool)result.Data)
            {
                Snackbar.Add($"Diagnosis successfully updated", Severity.Success);
                await OnInitializedAsync();
            }
        }
        else
        {
            Snackbar.Add($"Oops! An error has occurred. This diagnosis is not in the database.", Severity.Error);
        }
    }
    private async Task RemoveDiagnosis(int diagnosisId)
    {
        var options = new DialogOptions
        {
            BackdropClick = false,
            MaxWidth = MaxWidth.Small,
            Position = DialogPosition.Center,
        };
        var dialog = await DialogService.ShowAsync<DeleteDialog>("Delete diagnosis", options);
        var result = await dialog.Result;
        if (!result.Canceled && (bool)result.Data)
        {
            var delete = await DiagnosisService.DeleteDiagnosisAsync(diagnosisId);
            if (delete)
            {
                Snackbar.Add($"Diagnosis successfully deleted", Severity.Success);
                await OnInitializedAsync();
            }
            else
            {
                Snackbar.Add($"Oops! An error has occurred. This Diagnosis is not in the database.", Severity.Error);
            }
        }
    }
}
