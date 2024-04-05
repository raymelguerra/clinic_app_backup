using ClinicApp.Core.Models;
using ClinicApp.WebApp.Interfaces;
using ClinicApp.WebApp.Services;
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
    [Inject] IPhysician PhysicianService { get; set; } = null!;

    [Parameter] public Contractor Model { get; set; } = null!;
    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = null!;

    private MudForm form = null!;
    private PhysicianValidator phyValidator = new();
    private PayrollValidator prValidator = new();
    Func<dynamic, string> converter = p => p?.Name;

    IEnumerable<Company> _companies = new List<Company>();
    IEnumerable<ContractorType> _contractorTypes = new List<ContractorType>();
    IEnumerable<Insurance> _insurances = new List<Insurance>();
    IEnumerable<Procedure> _procedures = new List<Procedure>();
    IEnumerable<Procedure> _procValids = new List<Procedure>();

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

    async void Submit()
    {
        await form!.Validate();
        if (!form.IsValid) return;

        try
        {
            Model.Payrolls = Model.Payrolls.Select(x => new Payroll
            {
                CompanyId = x.CompanyId,
                ContractorId = x.ContractorId,
                ContractorTypeId = x.ContractorType.Id,
                InsuranceProcedureId = x.InsuranceProcedureId,
            }).ToList();
            var result = MudDialog!.Title.Contains("Add") ? await PhysicianService.PostPhysicianAsync(Model!) : await PhysicianService!.PutPhysicianAsync(Model!.Id, Model);
            if (result)
                MudDialog!.Close(DialogResult.Ok(true));
            else
                Snackbar!.Add($"Oops, there was an error adding a new physician.", Severity.Error);
        }
        catch (Exception ex)
        {
            Snackbar!.Add($"Oops, an error occurred. The error type is: {ex.Message}.", Severity.Error);
        }
    }
    void Cancel() => MudDialog.Cancel();

    #region Table payroll methods

    private void AddRow()
    {
        this.Model?.Payrolls?.Add(new Payroll
        {
            CompanyId = _companies.First().Id,
        });
        StateHasChanged();
    }

    private bool HaveNewAgreement()
    {
        return Model.Payrolls.Any(x => !prValidator.Validate(x).IsValid);
        //if (Model.Payrolls.Count > 0)
        //{
        //    bool isValid = true;
        //    int i = 0;
        //    while (isValid && i < Model.Payrolls.Count)
        //    {
        //        isValid = prValidator.Validate(Model.Payrolls[i]).IsValid;
        //        i++;
        //    }
        //    return isValid;
        //}
        //return true;
    }

    private void BackupItem(object element)
    {
        var orignValue = ((Payroll)element).InsuranceProcedure;
        ((Payroll)element).InsuranceProcedure ??= new();

        //((Payroll)element).Company ??= new();
        //((Payroll)element).ContractorType ??= null;

        elementBeforeEdit = new()
        {
            Company = ((Payroll)element).Company,
            ContractorType = ((Payroll)element).ContractorType,
            InsuranceProcedure = orignValue,
        };
        //  AddEditionEvent($"RowEditPreview event: made a backup of Element {((Payroll)element).Company.Name}");
    }

    private void ItemHasBeenCommitted(object element)
    {
        var elt = (Payroll)element;
        try
        {
            var insProc = _insurances.FirstOrDefault(x => x.Id == elt.InsuranceProcedure.Insurance.Id).InsuranceProcedures;
            elt.InsuranceProcedureId = insProc.FirstOrDefault(x => x.InsuranceId == elt.InsuranceProcedure.Insurance.Id && x.ProcedureId == elt.InsuranceProcedure.Procedure.Id).Id;
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
        ((Payroll)element).Company = elementBeforeEdit.Company;
        ((Payroll)element).ContractorType = elementBeforeEdit.ContractorType;
        ((Payroll)element).InsuranceProcedure = elementBeforeEdit.InsuranceProcedure;
        // AddEditionEvent($"RowEditCancel event: Editing of Element {((Payroll)element).Company.Name} canceled");
    }

    private IEnumerable<int> validProcIds = new List<int>();
    private void OnInsuranceChanged(string insurance)
    {
        if (insurance != null)
        {
            bool changeVal = elementBeforeEdit.InsuranceProcedure != null &&  elementBeforeEdit.InsuranceProcedure.Insurance.Name != insurance;
            if (changeVal)
            {
                if (selectCtrType != null)
                    selectCtrType.Clear();
                if (selProc != null)
                    selProc.Clear();
            }
            validProcIds = _insurances.First(x => x.Name == insurance).InsuranceProcedures.Select(x => x.ProcedureId);
            _procValids = new List<Procedure>();
        }
    }

    MudSelect<ContractorType> selectCtrType;
    MudSelect<Procedure> selProc;
    private void OnContractorTypeChanged(string type)
    {
        if (type != null)
        {
            bool changeVal = elementBeforeEdit.InsuranceProcedure != null && elementBeforeEdit.ContractorType.Name != type;
            if (selProc != null && changeVal)
                selProc.Clear();
            var ctrId = _contractorTypes.First(x => x.Name == type).Id;
            _procValids = _procedures.Where(x => x.ContractorTypeId == ctrId && validProcIds.Contains(x.Id)).ToList();
        }
    }

    private void DeleteItem(object element) {

        var payroll = (Payroll)element;
        Model.Payrolls.Remove(payroll);
        StateHasChanged();
    }

    #endregion
}
