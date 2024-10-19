using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Dto.Application;
using ClinicApp.WebApp.Interfaces;
using ClinicApp.WebApp.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ClinicApp.WebApp.Pages;

public partial class PeriodPaymentDetailPage : ComponentBase
{
    [Inject] IDialogService DialogService { get; set; } = null!;
    [Inject] NavigationManager NavigationManager { get; set; } = null!;

    [Inject] IPeriod PeriodService { get; set; } = null!;
    [Inject] IInsurance InsuranceService { get; set; } = null!;
    [Inject] IPhysician PhysicianService { get; set; } = null!;
    [Inject] IServiceLog ServiceLogService { get; set; } = null!;
    [Inject] IClient ClientService { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;


    [Parameter] public int PeriodId { get; set; }
    private IEnumerable<Insurance>? _insurances;
    private IEnumerable<Contractor>? _contractors;
    private Period _period = new Period();
    private Insurance? insuranceSelect = null;
    private Contractor? _contractorSelect = null;
    private IEnumerable<Client>? _clients;
    private IEnumerable<ServiceLog>? _serviceLogs;
    private bool _loading = false;

    private PeriodCalculationResultDto calculationResult = new();

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var periodProm = PeriodService.GetPeriodAsync(PeriodId);
            var insProm = InsuranceService.GetInsuranceAsync("");
            var profitProm = ServiceLogService.CalculatePeriodAsync(PeriodId);

            await Task.WhenAll([periodProm, insProm, profitProm]);

            _period = periodProm.Result!;
            _insurances = insProm.Result!;
            calculationResult = profitProm.Result!;
        }
        catch (Exception ex)
        {
            Snackbar!.Add($"Oops, there was an error loading the page: {ex.Message}.", Severity.Error);
        }
    }

    Func<dynamic, string> converter = p => p?.Name;

    private async Task GoBack()
    {
        NavigationManager.NavigateTo("/periodpayment", true);
    }

    #region Insurance search
    MudAutocomplete<Contractor>? selContractor;
    private async void OnInsuranceChanged(string insurance)
    {
        if (insurance != null)
        {
            if (selContractor != null)
            {
                await selContractor.ClearAsync();
            }

            if (_insurances == null || _insurances.Count() == 0 || insuranceSelect == null || insuranceSelect.Name != insurance)
                return;

            _contractors = await PhysicianService.GetContractorsByInsurance(insuranceSelect.Id);
        }
    }
    #endregion

    #region Contractor Search
    private async Task<IEnumerable<Contractor>> SearchContractor(string value, CancellationToken token)
    {
        if (token.IsCancellationRequested) return Enumerable.Empty<Contractor>();

        if (string.IsNullOrEmpty(value))
            return _contractors;

        return _contractors!.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }
    #endregion

    #region Service log step

    private async void SearchClientByInsuranceAndContractor()
    {
        if (_contractorSelect == null || insuranceSelect == null) return;
        _clients = await ClientService.GetClientsByContractorAndInsurance(_contractorSelect.Id, insuranceSelect.Id);

        _serviceLogs = [];

        if (_clients != null && _clients.Count() > 0)
            ClientSelected(_clients.ElementAt(0).Id);

        StateHasChanged();
    }

    private async void ClientSelected(int clientId)
    {
        _loading = true;
        _serviceLogs = await ServiceLogService.GetAllServiceLogsByClientContractorPeriodInsurance(clientId, _contractorSelect!.Id, _period.Id, insuranceSelect!.Id);
        _loading = false;
        StateHasChanged();
    }
    #endregion

}
