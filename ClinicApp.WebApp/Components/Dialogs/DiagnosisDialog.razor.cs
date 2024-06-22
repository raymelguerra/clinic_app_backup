using ClinicApp.Core.Models;
using ClinicApp.WebApp.Interfaces;
using ClinicApp.WebApp.Services;
using ClinicApp.WebApp.Services.Validations;
using Microsoft.AspNetCore.Components;
using MudBlazor;
namespace ClinicApp.WebApp.Components.Dialogs;

public partial class DiagnosisDialog : ComponentBase
{
    [Parameter]
    public Diagnosis Model { get; set; } = null!;
    [Inject] ISnackbar Snackbar { get; set; } = null!;
    [Inject] IDiagnosis DiagnosisService { get; set; } = null!;
    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = null!;

    private MudForm? form;
    private DiagnosisValidator diagnosisValidator = new();

    protected override Task OnParametersSetAsync()
    {
        return base.OnParametersSetAsync();
    }

    async void Submit()
    {
        await form!.Validate();
        if (!form.IsValid) return;

        try
        {

            var result = MudDialog!.Title.Contains("Add") ? await DiagnosisService.PostDiagnosisAsync(Model!) : await DiagnosisService!.PutDiagnosisAsync(Model!.Id, Model);
            if (result)
                MudDialog!.Close(DialogResult.Ok(true));
            else
                Snackbar!.Add($"Oops, there was an error adding a new Diagnosis.", Severity.Error);
        }
        catch (Exception ex)
        {
            Snackbar!.Add($"Oops, an error occurred. The error type is: {ex.Message}.", Severity.Error);
        }
    }
    void Cancel() => MudDialog.Cancel();

}
