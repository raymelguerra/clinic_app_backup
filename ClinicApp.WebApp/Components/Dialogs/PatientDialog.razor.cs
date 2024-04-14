using ClinicApp.Core.Models;
using ClinicApp.WebApp.Interfaces;
using ClinicApp.WebApp.Services.Validations;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Internal;
using System.Runtime.CompilerServices;

namespace ClinicApp.WebApp.Components.Dialogs;

public partial class PatientDialog : ComponentBase
{
    [Inject] private IClient ClientService { get; set; } = null!;
    [Inject] private IDiagnosis? DiagnosisService { get; set; }
    [Inject] private IReleaseInformation? ReleaseInformationService { get; set; }
    [Inject] private IPhysician? PhysicianService { get; set; }
    [Inject] private ICompany? CompanyService { get; set; }
    [Inject] ISnackbar? Snackbar { get; set; }
    [Inject] IDialogService? DialogService { get; set; }

    [Parameter]
    public Client? Model { get; set; }
    [CascadingParameter] MudDialogInstance? MudDialog { get; set; }

    public IEnumerable<ReleaseInformation>? _releaseInformations { get; set; }
    public IEnumerable<Diagnosis>? _diagnoses { get; set; }
    public IEnumerable<Contractor>? _contractors { get; set; }
    public IEnumerable<Company>? _companies { get; set; }

    public Contractor ctrSelected { get; set; } // = new();

    private MudForm? form;
    private PatientValidator? ptValidator = new();

    // Agreements table
    private Agreement? SelectedItem { get; set; } = new();
    private Agreement? elementBeforeEdit { get; set; }

    Func<dynamic, string> converter = p => p?.Name;
    Func<Payroll, string> converterPy = p => p?.InsuranceProcedure != null ? $"{p?.InsuranceProcedure.Insurance.Name} | {p?.InsuranceProcedure.Procedure.Name}" : string.Empty;
    protected override async Task OnParametersSetAsync()
    {
        var relInf = ReleaseInformationService!.GetReleaseInformationAsync("");
        var diag = DiagnosisService!.GetDiagnosisAsync("");
        var ctr = PhysicianService!.GetPhysicianAsync("");
        var cmp = CompanyService!.GetCompanyAsync("");

        await Task.WhenAll(relInf, diag, ctr, cmp);

        _releaseInformations = relInf.Result;
        _diagnoses = diag.Result;
        _contractors = ctr.Result;
        _companies = cmp.Result;

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
    private async Task<IEnumerable<Contractor>> SearchContractor(string value)
    {
        // In real life use an asynchronous function for fetching data from an api.
        await Task.Delay(5);

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return _contractors;
        return _contractors!.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }
    #endregion

    #region Table agreements methods

    private void AddRow()
    {
        this.Model.Agreements.Add(new Agreement
        {
            CompanyId = _companies.First().Id,
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
    }

    private void ItemHasBeenCommitted(object element)
    {
        // AddEditionEvent($"RowEditCommit event: Changes to Element {((Agreement)element).CompanyId} committed");
    }

    private void ResetItemToOriginalValues(object element)
    {
        ((Agreement)element).Payroll = elementBeforeEdit.Payroll;
        ((Agreement)element).RateEmployees = elementBeforeEdit.RateEmployees;
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
