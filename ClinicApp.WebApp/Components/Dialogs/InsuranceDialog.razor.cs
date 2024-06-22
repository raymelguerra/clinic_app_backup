using ClinicApp.Core.Models;
using ClinicApp.WebApp.Interfaces;
using ClinicApp.WebApp.Services;
using ClinicApp.WebApp.Services.Validations;
using Microsoft.AspNetCore.Components;
using MudBlazor;
namespace ClinicApp.WebApp.Components.Dialogs;

public partial class InsuranceDialog : ComponentBase
{
    [Parameter]
    public Insurance Model { get; set; } = null!;
    [Inject] ISnackbar Snackbar { get; set; } = null!;
    [Inject] IInsurance InsuranceService { get; set; } = null!;
    [Inject] IProcedure ProcedureServices { get; set; } = null!;
    [Inject] IContractorType ContractorTypeService { get; set; } = null!;
    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = null!;

    private MudForm? form;
    private InsuranceValidator insValidator = new();
    private InsuranceProcedureValidator insProcValidator = new();
    Func<Procedure, string> converter = p =>
    {
        if (p == null)
        {
            return string.Empty;
        }

        string result = p.Name;
        if (p.ContractorType != null)
        {
            result += " | " + p.ContractorType.Name;
        }
        return result;
    };

    // Procedures table
    private InsuranceProcedure SelectedItem { get; set; } = new();
    private InsuranceProcedure? elementBeforeEdit { get; set; }
    private IEnumerable<Procedure> _procedures = new List<Procedure>();
    private IEnumerable<ContractorType> _contractorTypes = new List<ContractorType>();

    protected override async Task OnInitializedAsync()
    {
        var proc = ProcedureServices.GetProcedureAsync("");
        var ctrType = ContractorTypeService.GetContractorTypeAsync("");

        await Task.WhenAll(proc, ctrType);

        _procedures = proc.Result;
        _contractorTypes = ctrType.Result;
    }
    protected override Task OnParametersSetAsync()
    {
        return base.OnParametersSetAsync();
    }

    async void Submit()
    {
        await form!.Validate();
        if (!form.IsValid) return;

        try
        {
            Model.InsuranceProcedures = Model.InsuranceProcedures!
                .Select(x => new InsuranceProcedure
                {
                    ProcedureId = x.ProcedureId,
                    InsuranceId = x.InsuranceId,
                    Rate = x.Rate,
                })
                .ToList();

            var result = MudDialog!.Title.Contains("Add") ? await InsuranceService.PostInsuranceAsync(Model!) : await InsuranceService!.PutInsuranceAsync(Model!.Id, Model);
            if (result)
                MudDialog!.Close(DialogResult.Ok(true));
            else
                Snackbar!.Add($"Oops, there was an error adding a new insurance.", Severity.Error);
        }
        catch (Exception ex)
        {
            Snackbar!.Add($"Oops, an error occurred. The error type is: {ex.Message}.", Severity.Error);
        }
    }
    void Cancel() => MudDialog.Cancel();

    #region Table procedures methods

    private void AddRow()
    {
        this.Model.InsuranceProcedures!.Add(new InsuranceProcedure
        {
            Procedure = new(),
        });
        StateHasChanged();
    }

    private void BackupItem(object element)
    {

        var orignValue = ((InsuranceProcedure)element).Procedure;
        ((InsuranceProcedure)element).Procedure ??= new();

        elementBeforeEdit = new()
        {
            Rate = ((InsuranceProcedure)element).Rate,
            Procedure = orignValue,
        };
    }

    private void ItemHasBeenCommitted(object element)
    {
        var elt = (InsuranceProcedure)element;
        try
        {
            elt.ProcedureId = _procedures.FirstOrDefault(x => x.Id == elt.Procedure.Id)!.Id;
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
        ((InsuranceProcedure)element).Rate = elementBeforeEdit!.Rate;
        ((UnitDetail)element).Procedure = elementBeforeEdit!.Procedure;
    }

    private bool HaveNewProcedure()
    {
        return Model.InsuranceProcedures!.Any(x => !insProcValidator.Validate(x).IsValid);
    }

    private void DeleteItem(object element)
    {

        var insproc = (InsuranceProcedure)element;
        Model.InsuranceProcedures!.Remove(insproc);
        StateHasChanged();
    }

    #endregion

}
