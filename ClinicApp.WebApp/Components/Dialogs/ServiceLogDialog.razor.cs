using ClinicApp.Core.Models;
using ClinicApp.WebApp.Services.Validations;
using Microsoft.AspNetCore.Components;
using MudBlazor;
namespace ClinicApp.WebApp.Components.Dialogs;

public partial class ServiceLogDialog : ComponentBase
{
    [Parameter]
    public ServiceLog Model { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }

    private MudForm form;
    private ServiceLogValidator slValidator = new();

    // UnitDetail table
    private UnitDetail SelectedItem { get; set; } = new();
    private UnitDetail elementBeforeEdit { get; set; }

    protected override Task OnParametersSetAsync()
    {
        return base.OnParametersSetAsync();
    }

    void Submit() => MudDialog.Close(DialogResult.Ok(true));
    void Cancel() => MudDialog.Cancel();

    #region Table unit detail methods

    private void AddRow()
    {
        this.Model.UnitDetails.Add(
            new UnitDetail
            {
                DateOfService = DateTime.Now,
                Id = 1,
                PlaceOfService = new PlaceOfService
                {
                    Name = "Home"
                },
                Unit = 18,
                Procedure = new Procedure
                {
                    Name = "H2014"
                }
            }
        );
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
            DateOfService = ((UnitDetail)element).DateOfService,
            PlaceOfService = ((UnitDetail)element).PlaceOfService,
            Procedure = ((UnitDetail)element).Procedure,
        };
        AddEditionEvent($"RowEditPreview event: made a backup of Element {((UnitDetail)element).PlaceOfService.Name}");
    }

    private void ItemHasBeenCommitted(object element)
    {
        AddEditionEvent($"RowEditCommit event: Changes to Element {((UnitDetail)element).PlaceOfService.Name} committed");
    }

    private void ResetItemToOriginalValues(object element)
    {
        ((UnitDetail)element).DateOfService = elementBeforeEdit.DateOfService;
        ((UnitDetail)element).PlaceOfService = elementBeforeEdit.PlaceOfService;
        ((UnitDetail)element).Procedure = elementBeforeEdit.Procedure;
        AddEditionEvent($"RowEditCancel event: Editing of Element {((UnitDetail)element).PlaceOfService.Name} canceled");
    }
    #endregion

}
