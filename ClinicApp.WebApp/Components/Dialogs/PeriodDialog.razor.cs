using ClinicApp.Core.Models;
using ClinicApp.WebApp.Interfaces;
using ClinicApp.WebApp.Services;
using ClinicApp.WebApp.Services.Validations;
using Microsoft.AspNetCore.Components;
using MudBlazor;
namespace ClinicApp.WebApp.Components.Dialogs;

public partial class PeriodDialog : ComponentBase
{
    [Parameter]
    public Period Model { get; set; } = null!;
    [Inject] ISnackbar Snackbar { get; set; } = null!;
    [Inject] IPeriod PeriodService { get; set; } = null!;
    [Inject] IProcedure ProcedureServices { get; set; } = null!;
    [Inject] IContractorType ContractorTypeService { get; set; } = null!;
    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = null!;

    private MudForm? form;
    private PeriodValidator periodValidator = new();

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

            var result = MudDialog!.Title.Contains("Add") ? await PeriodService.PostPeriodAsync(Model!) : await PeriodService!.PutPeriodAsync(Model!.Id, Model);
            if (result)
                MudDialog!.Close(DialogResult.Ok(true));
            else
                Snackbar!.Add($"Oops, there was an error adding a new Period.", Severity.Error);
        }
        catch (Exception ex)
        {
            Snackbar!.Add($"Oops, an error occurred. The error type is: {ex.Message}.", Severity.Error);
        }
    }
    void Cancel() => MudDialog.Cancel();

}
