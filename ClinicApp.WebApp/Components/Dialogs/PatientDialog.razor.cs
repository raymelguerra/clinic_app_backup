using ClinicApp.Core.Models;
using ClinicApp.WebApp.Services.Validations;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using static MudBlazor.Colors;
namespace ClinicApp.WebApp.Components.Dialogs;

public partial class PatientDialog : ComponentBase
{
    [Parameter]
    public Client Model { get; set; }
    public IEnumerable<PatientAccount> PatientAccountList { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }
    [Inject] IDialogService DialogService { get; set; }

    private MudForm form;
    private PatientValidator ptValidator = new();
    // private MudDialog dialog;
    private MudBlazor.Internal.MudInputAdornment dialog;

    // Agreements table
    private Agreement SelectedItem { get; set; } = new();
    private Agreement elementBeforeEdit { get; set; }

    protected override Task OnParametersSetAsync()
    {
        PatientAccountList = new List<PatientAccount> {
            //new PatientAccount {
            //    Id = 1,
            //    ClientId = 1,
            //    CreateDate = DateTime.Now,
            //    ExpireDate = DateTime.Now,
            //    LicenseNumber = "pepepep"
            //},
            //new PatientAccount {
            //    Id = 2,
            //    ClientId = 1,
            //    CreateDate = DateTime.Now,
            //    ExpireDate = DateTime.Now,
            //    LicenseNumber = "123123125439"
            //},
            //new PatientAccount {
            //    Id = 2,
            //    ClientId = 1,
            //    CreateDate = DateTime.Now,
            //    ExpireDate = DateTime.Now,
            //    LicenseNumber = "DOES NOT APPLY",
            //    Auxiliar = "HG883747"
            //},
        };
        return base.OnParametersSetAsync();
    }

    void Submit() => MudDialog.Close(DialogResult.Ok(true));
    void Cancel() => MudDialog.Cancel();

    #region Contractor Search
    private async Task<IEnumerable<string>> SearchContractor(string value)
    {
        // In real life use an asynchronous function for fetching data from an api.
        await Task.Delay(5);

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return states;
        return states.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }
    private string[] states =
  {
        "Alabama", "Alaska", "American Samoa", "Arizona",
        "Arkansas", "California", "Colorado", "Connecticut",
        "Delaware", "District of Columbia", "Federated States of Micronesia",
        "Florida", "Georgia", "Guam", "Hawaii", "Idaho",
        "Illinois", "Indiana", "Iowa", "Kansas", "Kentucky",
        "Louisiana", "Maine", "Marshall Islands", "Maryland",
        "Massachusetts", "Michigan", "Minnesota", "Mississippi",
        "Missouri", "Montana", "Nebraska", "Nevada",
        "New Hampshire", "New Jersey", "New Mexico", "New York",
        "North Carolina", "North Dakota", "Northern Mariana Islands", "Ohio",
        "Oklahoma", "Oregon", "Palau", "Pennsylvania", "Puerto Rico",
        "Rhode Island", "South Carolina", "South Dakota", "Tennessee",
        "Texas", "Utah", "Vermont", "Virgin Island", "Virginia",
        "Washington", "West Virginia", "Wisconsin", "Wyoming",
    };

    #endregion

    #region Table agreements methods

    private void AddRow()
    {
        this.Model.Agreements.Add(new Agreement
        {
            Company = new Company
            {
                Id = 1,
                Name = "Expanding Possibilities",
            },
            Payroll = new Payroll
            {
                Contractor = new(),
                ContractorId = 1,
                ContractorType = new(),
                Procedure = new()
            },
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
            Payroll = ((Agreement)element).Payroll,
            RateEmployees = ((Agreement)element).RateEmployees,
        };
        AddEditionEvent($"RowEditPreview event: made a backup of Element {((Agreement)element).Company.Name}");
    }

    private void ItemHasBeenCommitted(object element)
    {
        AddEditionEvent($"RowEditCommit event: Changes to Element {((Agreement)element).Company.Name} committed");
    }

    private void ResetItemToOriginalValues(object element)
    {
        ((Agreement)element).Payroll = elementBeforeEdit.Payroll;
        ((Agreement)element).RateEmployees = elementBeforeEdit.RateEmployees;
        AddEditionEvent($"RowEditCancel event: Editing of Element {((Agreement)element).Company.Name} canceled");
    }
    #endregion

    #region Patient account
    private async Task ShowPatienAccountDialog()
    {
        var parameters = new DialogParameters<PatientAccountDialog> { { x => x.PatientAccountList, PatientAccountList } };
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            FullWidth = true,
            DisableBackdropClick = false,
            Position = DialogPosition.TopCenter,
        };

        var result = await DialogService.ShowAsync<PatientAccountDialog>("Action Patient Account", parameters, options);
    }
    #endregion
}
