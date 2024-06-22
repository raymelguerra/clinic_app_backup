using ClinicApp.Core.Models;
using ClinicApp.WebApp.Interfaces;
using ClinicApp.WebApp.Services.Validations;
using Microsoft.AspNetCore.Components;
using MudBlazor;
namespace ClinicApp.WebApp.Components.Dialogs;

public partial class ProcedureDialog : ComponentBase
{
    [Parameter]
    public Procedure Model { get; set; } = null!;
    [Inject] ISnackbar Snackbar { get; set; } = null!;
    [Inject] IProcedure ProcedureService { get; set; } = null!;
    [Inject] IContractorType ContractorTypeService { get; set; } = null!;
    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = null!;

    private MudForm? form;
    private ProcedureValidator procedureValidator = new();

    private IEnumerable<ContractorType> _contractorTypes = new List<ContractorType>();
    Func<ContractorType, string> converter = p => p?.Name;

    protected async override Task OnParametersSetAsync()
    {
        _contractorTypes = await ContractorTypeService.GetContractorTypeAsync("");
    }

    async void Submit()
    {
        await form!.Validate();
        if (!form.IsValid) return;

        try
        {
            Model.ContractorTypeId = Model.ContractorType!.Id;
            Model.ContractorType = null;

            var result = MudDialog!.Title.Contains("Add") ? await ProcedureService.PostProcedureAsync(Model!) : await ProcedureService!.PutProcedureAsync(Model!.Id, Model);
            if (result)
                MudDialog!.Close(DialogResult.Ok(true));
            else
                Snackbar!.Add($"Oops, there was an error adding a new Procedure.", Severity.Error);
        }
        catch (Exception ex)
        {
            Snackbar!.Add($"Oops, an error occurred. The error type is: {ex.Message}.", Severity.Error);
        }
    }
    void Cancel() => MudDialog.Cancel();

}
