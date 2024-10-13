using ClinicApp.Core.Models;
using ClinicApp.WebApp.Components.Dialogs;
using ClinicApp.WebApp.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ClinicApp.WebApp.Pages;

public partial class PendingPage : ComponentBase
{
    [Inject] IDialogService? DialogService { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] IServiceLog ServiceLogService { get; set; } = null!;
    [Inject] IPhysician PhysicianService { get; set; } = null!;
    [Inject] IClient PatientService { get; set; } = null!;

    IEnumerable<ServiceLog> ServiceLogs = new List<ServiceLog>();
    private bool _loading = false;



    protected override async Task OnInitializedAsync()
    {

        await base.OnInitializedAsync();
        try
        {
            _loading = true;
            if(ServiceLogService == null)
            {
                throw new Exception("Internal Server error");
            }

            var servLogProm = ServiceLogService.GetServiceLogAsync("$filter=Pending ne null and Pending ne ''");

            await Task.WhenAll(servLogProm);

            ServiceLogs = servLogProm.Result;
            
        }
        finally
        {
            _loading = false;
        }


    }
    private async Task ShowDialog(int Id, PendingBy type)
    {
        if (type == PendingBy.PHYSICIAN)
        {
            if (PhysicianService == null || DialogService == null) {
                throw new Exception("Internal server error");
            }
            var data = await PhysicianService.GetPhysicianAsync(Id);
            if (data != null)
            {
                var parameters = new DialogParameters<PhysicianDialog> { { x => x.Model, data } };
                var dialog = await DialogService.ShowAsync<PhysicianDialog>("Edit Physician", parameters);
                var result = await dialog.Result;
                if (!result.Canceled && (bool)result.Data)
                {
                    Snackbar.Add($"Physician successfully updated", Severity.Success);
                    await OnInitializedAsync();
                }
            }
        }
        else
        {
            if (PatientService == null || DialogService == null)
            {
                throw new Exception("Internal server error");
            }
            var data = await PatientService.GetClientAsync(Id);
            if (data != null)
            {
                var parameters = new DialogParameters<PatientDialog> { { x => x.Model, data } };
                var dialog = await DialogService.ShowAsync<PatientDialog>("Edit Patient", parameters);
                var result = await dialog.Result;
                if (!result.Canceled && (bool)result.Data)
                {
                    Snackbar.Add($"Patient successfully updated", Severity.Success);
                    await OnInitializedAsync();
                }
            }
        }
    }

    private async Task RemovePending(int serviceLog)
    {
        var options = new DialogOptions
        {
            BackdropClick = false,
            MaxWidth = MaxWidth.Small,
            Position = DialogPosition.Center,
        };
        var dialog = await DialogService.ShowAsync<DeleteDialog>("Delete Service Log Pending", options);
        var result = await dialog.Result;
        if (!result.Canceled && (bool)result.Data)
        {
            var serviceLogFull = await ServiceLogService.GetServiceLogAsync(serviceLog);
            if (serviceLogFull == null) {
                Snackbar.Add($"Oops! An error has occurred. This service log is not in the database.", Severity.Error);
            }
            serviceLogFull.Pending = "";

            var deletePending = await ServiceLogService.PutServiceLogAsync(serviceLog, serviceLogFull);
            if (deletePending)
            {
                Snackbar.Add($"Issue successfully deleted", Severity.Success);
                await OnInitializedAsync();
            }
            else
            {
                Snackbar.Add($"Oops! An error has occurred. This issue is not in the database.", Severity.Error);
            }
        }
    }

    private enum PendingBy
    {
        PHYSICIAN, PATIENT
    }
}
