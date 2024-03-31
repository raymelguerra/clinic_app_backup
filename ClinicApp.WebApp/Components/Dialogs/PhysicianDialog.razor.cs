using ClinicApp.Core.Models;
using ClinicApp.WebApp.Interfaces;
using ClinicApp.WebApp.Services.Validations;
using Microsoft.AspNetCore.Components;
using MudBlazor;
namespace ClinicApp.WebApp.Components.Dialogs;

public partial class PhysicianDialog : ComponentBase
{
    [Inject] ISnackbar Snackbar { get; set; } = null!;
    [Inject] ICompany CompanyService { get; set; } = null!;
    [Inject] IContractorType ContractorTypeService { get; set; } = null!;
    [Inject] IInsurance InsuranceService { get; set; } = null!;
    [Inject] IProcedure ProcedureService { get; set; } = null!;

    [Parameter] public Contractor Model { get; set; } = null!;
    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = null!;

    private MudForm form = null!;
    private PhysicianValidator phyValidator = new();
    Func<dynamic, string> converter = p => p?.Name;

    IEnumerable<Company> _companies = new List<Company>();
    IEnumerable<ContractorType> _contractorTypes = new List<ContractorType>();
    IEnumerable<Insurance> _insurances = new List<Insurance>();
    IEnumerable<Procedure> _procedures = new List<Procedure>();

    // Payroll table
    private Payroll SelectedItem { get; set; } = new();
    private Payroll elementBeforeEdit { get; set; } = null!;

    protected async override Task OnParametersSetAsync()
    {
        var compTask = CompanyService.GetCompanyAsync("");
        var ctrTypeTask = ContractorTypeService.GetContractorTypeAsync("");
        var insTask = InsuranceService.GetInsuranceAsync("");
        var procTask = ProcedureService.GetProcedureAsync("");

        await Task.WhenAll(compTask, ctrTypeTask, insTask, procTask);

        _companies = compTask.Result;
        _contractorTypes = ctrTypeTask.Result;
        _insurances = insTask.Result;
        _procedures = procTask.Result;

    }

    void Submit() => MudDialog.Close(DialogResult.Ok(true));
    void Cancel() => MudDialog.Cancel();

    #region Table payroll methods

    private void AddRow()
    {
        this.Model?.Payrolls?.Add(new());
        StateHasChanged();
    }

    private bool HaveNewAgreement()
    {
        return Model?.Payrolls?.Any(x => x.Equals(default(Payroll))) ?? false;
    }

    private void AddEditionEvent(string message)
    {
        StateHasChanged();
    }

    private void BackupItem(object element)
    {
        ((Payroll)element).InsuranceProcedure ??= new InsuranceProcedure
        {
            Insurance = new(),
            Procedure = new()
        };
        ((Payroll)element).Company ??= new();
        ((Payroll)element).ContractorType ??= new();

        elementBeforeEdit = new()
        {
            Company = ((Payroll)element).Company,
            ContractorType = ((Payroll)element).ContractorType,
            InsuranceProcedure = ((Payroll)element).InsuranceProcedure,
        };
        //  AddEditionEvent($"RowEditPreview event: made a backup of Element {((Payroll)element).Company.Name}");
    }

    private void ItemHasBeenCommitted(object element)
    {
        var elt = (Payroll) element;
        AddEditionEvent($"RowEditCommit event: Changes to Element {((Payroll)element).Company.Name} committed");
        Console.WriteLine($"Payroll insurance: {string.Join(",",Model.Payrolls.Select(x=> x.InsuranceProcedure.Insurance.Name).ToList())}");
    }

    private void ResetItemToOriginalValues(object element)
    {
        ((Payroll)element).Company = elementBeforeEdit.Company;
        ((Payroll)element).ContractorType = elementBeforeEdit.ContractorType;
        ((Payroll)element).InsuranceProcedure = elementBeforeEdit.InsuranceProcedure;
        AddEditionEvent($"RowEditCancel event: Editing of Element {((Payroll)element).Company.Name} canceled");
    }

    private void OnInsuranceChanged(string insurance)
    {
        Console.WriteLine($"Code: {insurance}");
    }
    #endregion

}
