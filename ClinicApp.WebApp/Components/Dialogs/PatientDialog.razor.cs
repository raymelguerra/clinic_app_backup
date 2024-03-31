using ClinicApp.Core.Models;
using ClinicApp.WebApp.Interfaces;
using ClinicApp.WebApp.Services.Validations;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Internal;

namespace ClinicApp.WebApp.Components.Dialogs;

public partial class PatientDialog : ComponentBase
{
    [Inject] private IClient ClientService { get; set; } = null!;
    [Inject] private IDiagnosis? DiagnosisService { get; set; }
    [Inject] private IReleaseInformation? ReleaseInformationService { get; set; }
    [Inject] ISnackbar? Snackbar { get; set; }
    [Inject] IDialogService? DialogService { get; set; }

    [Parameter]
    public Client? Model { get; set; }
    [CascadingParameter] MudDialogInstance? MudDialog { get; set; }

    public IEnumerable<ReleaseInformation>? _releaseInformations { get; set; }
    public IEnumerable<Diagnosis>? _diagnoses { get; set; }


    private MudForm? form;
    private PatientValidator? ptValidator = new();

    // Agreements table
    private Agreement? SelectedItem { get; set; } = new();
    private Agreement? elementBeforeEdit { get; set; }

    Func<dynamic, string> converter = p => p?.Name;
    protected override async Task OnParametersSetAsync()
    {
        var relInf = ReleaseInformationService!.GetReleaseInformationAsync("");
        var diag = DiagnosisService!.GetDiagnosisAsync("");

        await Task.WhenAll(relInf, diag);

        _releaseInformations = relInf.Result;
        _diagnoses = diag.Result;

    }

    async void Submit()
    {
        await form!.Validate();
        if (!form.IsValid) return;

        try
        {
            Model!.DiagnosisId = Model.Diagnosis.Id;
            Model!.ReleaseInformationId = Model.ReleaseInformation.Id;

            var result = MudDialog!.Title.Contains("Add") ? await ClientService!.PostClientAsync(Model!) : await ClientService!.PutClientAsync(Model!.Id, Model);
            if (result)
                MudDialog!.Close(DialogResult.Ok(true));
            else
                Snackbar!.Add($"Oops, there was an error adding a new patient.", Severity.Error);
        }
        catch (Exception ex)
        {
            Snackbar!.Add($"Oops, an error occurred. The error type is: {ex.Message}.", Severity.Error);
        }
    }

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
            Payroll = new(),
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
        var parameters = new DialogParameters<PatientAccountDialog> { { x => x.Model, Model!.PatientAccounts ?? new PatientAccount() } };
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            FullWidth = true,
            DisableBackdropClick = false,
            Position = DialogPosition.TopCenter,
        };

        var dialog = await DialogService.ShowAsync<PatientAccountDialog>("Action Patient Account", parameters, options);
        var result = await dialog.Result;
        if (!result.Canceled && result.Data != null)
        {
            Model.PatientAccounts = (PatientAccount)result.Data;

            Model.AuthorizationNumber = Model.PatientAccounts.LicenseNumber != string.Empty ? Model.PatientAccounts.LicenseNumber : Model.PatientAccounts.Auxiliar;
            StateHasChanged();
        }

    }
    #endregion
}
