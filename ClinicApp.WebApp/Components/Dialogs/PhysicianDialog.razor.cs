using ClinicApp.Core.Models;
using ClinicApp.WebApp.Services.Validations;
using Microsoft.AspNetCore.Components;
using MudBlazor;
namespace ClinicApp.WebApp.Components.Dialogs;

public partial class PhysicianDialog : ComponentBase
{
    [Parameter]
    public Contractor Model { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }

    private MudForm form;
    private PhysicianValidator phyValidator = new();

    // Payroll table
    private Payroll SelectedItem { get; set; } = new();
    private Payroll elementBeforeEdit { get; set; }

    protected override Task OnParametersSetAsync()
    {
        return base.OnParametersSetAsync();
    }

    void Submit() => MudDialog.Close(DialogResult.Ok(true));
    void Cancel() => MudDialog.Cancel();

    #region Table payroll methods

    private void AddRow()
    {
        this.Model.Payrolls.Add(new Payroll
        {
            Company = new Company
            {
                Id = 1,
                Name = "Expanding possibilities",
            }
        });
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
            Company = ((Payroll)element).Company,
            ContractorType = ((Payroll)element).ContractorType,
            Procedure = ((Payroll)element).Procedure,
        };
        AddEditionEvent($"RowEditPreview event: made a backup of Element {((Payroll)element).Company.Name}");
    }

    private void ItemHasBeenCommitted(object element)
    {
        AddEditionEvent($"RowEditCommit event: Changes to Element {((Payroll)element).Company.Name} committed");
    }

    private void ResetItemToOriginalValues(object element)
    {
        ((Payroll)element).Company = elementBeforeEdit.Company;
        ((Payroll)element).ContractorType = elementBeforeEdit.ContractorType;
        ((Payroll)element).Procedure = elementBeforeEdit.Procedure;
        AddEditionEvent($"RowEditCancel event: Editing of Element {((Payroll)element).Company.Name} canceled");
    }
    #endregion

}
