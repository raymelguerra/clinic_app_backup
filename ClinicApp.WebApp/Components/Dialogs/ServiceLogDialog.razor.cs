using ClinicApp.Core.Models;
using ClinicApp.WebApp.Interfaces;
using ClinicApp.WebApp.Services.Validations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;


namespace ClinicApp.WebApp.Components.Dialogs;

public partial class ServiceLogDialog : ComponentBase
{
    [Parameter]
    public ServiceLog Model { get; set; }
    [Inject] ISnackbar Snackbar { get; set; }
    [CascadingParameter] MudDialogInstance MudDialog { get; set; }

    [Inject ] private IServiceLog ServiceLogService { get; set; }
    [Inject] private IInsurance InsuranceService { get; set; }
    [Inject] private IPayroll PayrollService { get; set; }
    [Inject] private IClient ClientService { get; set; }
    [Inject] private IPeriod PeriodService { get; set; }
    [Inject] private IPlaceOfService PlaceOfService { get; set; }

    private MudForm form;
    private ServiceLogValidator slValidator = new();
    private UnitValidator udValidator = new();
    IList<IBrowserFile> _files = new List<IBrowserFile>();

    private IEnumerable<Insurance> _insurances;
    private IEnumerable<Payroll> _payrolls;
    private IEnumerable<Client> _clients;
    private IEnumerable<Period> _periods;
    private IEnumerable<Contractor> _contractors;
    private IEnumerable<PlaceOfService> _placeOfService;

    Func<dynamic, string> converter = p => p?.Name;
    Func<Period, string> converterPeriod = p => p?.PayPeriod;
    Func<Payroll, string> converterPy = p => p?.InsuranceProcedure != null ? $"{p?.Contractor.Name} | {p?.InsuranceProcedure.Insurance.Name} | {p?.InsuranceProcedure.Procedure.Name}" : string.Empty;

    // UnitDetail table
    private UnitDetail SelectedItem { get; set; } = new();
    private UnitDetail elementBeforeEdit { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        var ins = InsuranceService.GetInsuranceAsync("");
        var perd = PeriodService.GetPeriodAsync("");
        var plOfServ = PlaceOfService.GetPlaceOfServiceAsync("");

        await Task.WhenAll(ins, perd, plOfServ);

        _insurances = ins.Result;
        _periods = perd.Result;
        _placeOfService = plOfServ.Result;

    }

    async void Submit()
    {
        await form!.Validate();
        if (!form.IsValid) return;

        try
        {
            Model!.ContractorId = Model.Contractor.Id;
            Model!.ClientId = Model.Client.Id;
            Model!.PeriodId = Model.Period.Id;
            Model!.InsuranceId = Model.Insurance.Id;

            if (Model.UnitDetails != null && Model.UnitDetails.Count() > 0) { 
                Model.UnitDetails = Model.UnitDetails.Select(x => new UnitDetail
                {
                    DateOfService = x.DateOfService,
                    PlaceOfServiceId = x.PlaceOfService.Id,
                    ProcedureId = x.Procedure.Id,
                    Unit = x.Unit,
                }).ToList();
            }

            var result = MudDialog!.Title.Contains("Add") ? await ServiceLogService!.PostServiceLogAsync(Model!) : await ServiceLogService!.PutServiceLogAsync(Model!.Id, Model);
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
    private async Task<IEnumerable<Contractor>> SearchContractor(string value, CancellationToken token)
    {
        if (string.IsNullOrEmpty(value))
            return _contractors;

        return _contractors!.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }
    #endregion

    #region Client Search
    private async Task<IEnumerable<Client>> SearchClient(string value, CancellationToken token)
    {
        if (string.IsNullOrEmpty(value))
            return _clients;

        return _clients!.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }
    #endregion

    #region Table unit detail methods

    private void AddRow()
    {
        this.Model.UnitDetails.Add(new());

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
    }

    private void ItemHasBeenCommitted(object element)
    {
        var ud = (UnitDetail)element;
        try
        {
            ud.ProcedureId = ud.Procedure.Id;
            ud.PlaceOfServiceId = ud.PlaceOfService.Id;
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
        ((UnitDetail)element).DateOfService = elementBeforeEdit.DateOfService;
        ((UnitDetail)element).PlaceOfService = elementBeforeEdit.PlaceOfService;
        ((UnitDetail)element).Procedure = elementBeforeEdit.Procedure;
    }

    private void DeleteItem(object element)
    {
        if (Model.UnitDetails == null || Model.UnitDetails.Count() == 0) return;
        
        var unitDetail = (UnitDetail)element;
        Model.UnitDetails!.Remove(unitDetail);
        StateHasChanged();
    }
    #endregion

    #region Changes dyamics in select
    MudAutocomplete<Contractor>? selContractor;
    MudAutocomplete<Client>? selClient;
    private async void OnInsuranceChanged(string insurance)
    {
        if (insurance != null)
        {
            if (selContractor != null) { 
                await selContractor.ClearAsync();
            }
            if (selClient != null)
            {
                await selClient.ClearAsync();
            }

            if (_insurances == null || _insurances.Count() == 0)
                return;

            var insId = _insurances.First(x => x.Name == insurance).Id;
            _payrolls = await PayrollService.GetPayrollsByInsuranceAsync(insId);
            // Remove all contract duplicates
            _contractors = _payrolls.GroupBy(x => x.ContractorId).Select(x => x.First()).Select(x => x.Contractor).ToList();
        }
    }

    private async void OnContractorChanged(string contractor)
    {
        if (contractor != null && contractor != string.Empty)
        {
            if (_contractors == null || _contractors.Count() == 0)
                return;

            if (selContractor != null)
                await selClient.ClearAsync();

            var ctrId = _contractors.First(x => x.Name == contractor).Id;

            if (Model.Insurance != null && Model.Contractor != null)
            {
                var allClients = await ClientService.GetClientsByContractorAndInsurance(Model.Contractor.Id, Model.Insurance.Id);
                // Remove all clients duplicates
                _clients = allClients.GroupBy(x => x.Id).Select(x => x.First());
            }
        }
    }

    private IEnumerable<Procedure> _getProceduresByAgreements(IEnumerable<Agreement> listAgg)
    {
        if (listAgg == null || listAgg.Count() == 0)
            return new List<Procedure>();
        var filterAgg = listAgg.Where(x => x.Payroll != null && x.Payroll.ContractorId == Model.Contractor.Id).ToList();
        var allProcedures = filterAgg.Select(x => x.Payroll.InsuranceProcedure.Procedure)
            .ToList();
        // Remove all procedures duplicates
        return allProcedures.GroupBy(x => x.Id).Select(x => x.First());
    }
    #endregion

    #region Load service by csv file
    private void UploadFiles(IBrowserFile file)
    {
        _files.Add(file);
        //TODO upload the files to the server
    }
    #endregion

    #region Pending
    private IEnumerable<string> MaxCharacters(string ch)
    {
        if (!string.IsNullOrEmpty(ch) && 250 < ch?.Length)
            yield return "Max 250 characters";
    }
    #endregion
}
