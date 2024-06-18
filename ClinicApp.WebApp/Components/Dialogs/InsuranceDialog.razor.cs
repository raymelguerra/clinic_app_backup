using ClinicApp.Core.Models;
using ClinicApp.WebApp.Services.Validations;
using Microsoft.AspNetCore.Components;
using MudBlazor;
namespace ClinicApp.WebApp.Components.Dialogs;

public partial class InsuranceDialog : ComponentBase
{
    [Parameter]
    public Insurance Model { get; set; } = null!;
    [Inject] ISnackbar Snackbar { get; set; } = null!;
    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = null!;

    private MudForm? form;
    private ServiceLogValidator slValidator = new();

    // Procedures table
    private InsuranceProcedure SelectedItem { get; set; } = new();
    private InsuranceProcedure? elementBeforeEdit { get; set; }

    protected override Task OnParametersSetAsync()
    {
        return base.OnParametersSetAsync();
    }

    void Submit() => MudDialog.Close(DialogResult.Ok(true));
    void Cancel() => MudDialog.Cancel();

    #region Table procedures methods

    private void AddRow()
    {
        StateHasChanged();
    }

    private void AddEditionEvent(string message)
    {
        StateHasChanged();
    }

    private void BackupItem(object element)
    {
        elementBeforeEdit = new()
        {
            Rate = ((InsuranceProcedure)element).Rate,
            Procedure = ((InsuranceProcedure)element).Procedure,
        };
        AddEditionEvent($"RowEditPreview event: made a backup of Element {((InsuranceProcedure)element).Procedure.Name}");
    }

    private void ItemHasBeenCommitted(object element)
    {
        AddEditionEvent($"RowEditCommit event: Changes to Element {((InsuranceProcedure)element).Procedure.Name} committed");
    }

    private void ResetItemToOriginalValues(object element)
    {
        ((InsuranceProcedure)element).Rate = elementBeforeEdit!.Rate;
        ((UnitDetail)element).Procedure = elementBeforeEdit!.Procedure;
        AddEditionEvent($"RowEditCancel event: Editing of Element {((InsuranceProcedure)element).Procedure.Name} canceled");
    }
    #endregion

}
