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
    [Inject] private ICompany? CompanyService { get; set; }
    [Inject] private IPayroll? PayrollService { get; set; }
    [Inject] ISnackbar? Snackbar { get; set; }
    [Inject] IDialogService? DialogService { get; set; }
    

    [Parameter]
    public Client? Model { get; set; }
    [CascadingParameter] MudDialogInstance? MudDialog { get; set; }

    public IEnumerable<ReleaseInformation>? _releaseInformations { get; set; }
    public IEnumerable<Diagnosis>? _diagnoses { get; set; }
    public IEnumerable<Contractor>? _contractors { get; set; }
    public IEnumerable<Company>? _companies { get; set; }
    public IEnumerable<Payroll>? _payrolls { get; set; }

    private MudForm? form;
    private PatientValidator? ptValidator = new();
    private AgreementValidator aggValidator = new();

    // Agreements table
    private Agreement? SelectedItem { get; set; } = new();
    private Agreement? elementBeforeEdit { get; set; }

    Func<dynamic, string> converter = p => p?.Name;
    Func<Payroll, string> converterPy = p => p?.InsuranceProcedure != null ? $"{p?.Contractor.Name} | {p?.InsuranceProcedure.Insurance.Name} | {p?.InsuranceProcedure.Procedure.Name}" : string.Empty;
    protected override async Task OnParametersSetAsync()
    {
        var relInf = ReleaseInformationService!.GetReleaseInformationAsync("");
        var diag = DiagnosisService!.GetDiagnosisAsync("");
        var cmp = CompanyService!.GetCompanyAsync("");
        var pyr = PayrollService!.GetPayrollAsync("");


        await Task.WhenAll(relInf, diag, cmp, pyr);

        _releaseInformations = relInf.Result;
        _diagnoses = diag.Result;
        _companies = cmp.Result;
        _payrolls = pyr.Result;

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
    private async Task<IEnumerable<Payroll>> SearchPayroll(string value, CancellationToken token)
    {
        if (string.IsNullOrEmpty(value))
            return _payrolls;
        // var completeName = 
        return _payrolls!.Where(x => $"{x.Contractor.Name} {x.InsuranceProcedure.Insurance.Name} {x.InsuranceProcedure.Procedure.Name}".Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }
    #endregion

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
            PayrollId = ((Agreement)element).PayrollId,
            RateEmployees = ((Agreement)element).RateEmployees,
        };
    }

    private void ItemHasBeenCommitted(object element)
    {
        var agg = (Agreement)element;
        try
        {
            agg.PayrollId = agg.Payroll.Id;
        }
        catch (Exception ex)
        {
            Snackbar!.Add($"Oops, an error occurred. The error type is: {ex.Message}.", Severity.Error);
        }
        finally
        {
            elementBeforeEdit = default!;
        }
    }

    private void ResetItemToOriginalValues(object element)
    {
        ((Agreement)element).Payroll = elementBeforeEdit.Payroll;
        ((Agreement)element).RateEmployees = elementBeforeEdit.RateEmployees;
        ((Agreement)element).PayrollId = elementBeforeEdit.PayrollId;
    }
    
    #region Patient account
    private async Task ShowPatienAccountDialog()
    {
        var parameters = new DialogParameters<PatientAccountDialog> { { x => x.Model, Model!.PatientAccounts ?? new PatientAccount() } };
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            FullWidth = true,
            BackdropClick = false,
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

    private void DeleteItem(object element)
    {

        var agg = (Agreement)element;
        Model.Agreements.Remove(agg);
        StateHasChanged();
    }
    #endregion
}
