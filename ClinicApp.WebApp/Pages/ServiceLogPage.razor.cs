using ClinicApp.Core.Models;
using ClinicApp.WebApp.Components.Dialogs;
using ClinicApp.WebApp.Interfaces;
using ClinicApp.WebApp.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Oauth2.sdk.Models;

namespace ClinicApp.WebApp.Pages;

public partial class ServiceLogPage : ComponentBase
{
    [Inject] IDialogService DialogService { get; set; }
    [Inject] IServiceLog ServiceLogService { get; set; }
    [Inject]
    private ISnackbar Snackbar { get; set; }

    public bool _loading = false;
    IEnumerable<ServiceLog> ServiceLogs = new List<ServiceLog>();

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _loading = true;
            ServiceLogs = await ServiceLogService.GetServiceLogAsync("");
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Oops, an error occurred. The error type is: {ex.Message}.", Severity.Error);
        }
        finally
        {
            _loading = false;
        }
    }
    private async Task AddServiceLog()
    {
        var sl = new ServiceLog();

        sl.UnitDetails = new List<UnitDetail>();

        var parameters = new DialogParameters<ServiceLogDialog> { { x => x.Model, sl } };

        var dialog = await DialogService.ShowAsync<ServiceLogDialog>("Add Service Log", parameters);
        var result = await dialog.Result;
        if (!result.Canceled && (bool)result.Data)
        {
            Snackbar.Add($"Service Log successfully created", Severity.Success);
            await OnInitializedAsync();
        }
    }

    private async Task EditServiceLog(int serviceLogId)
    {
        var sl = await ServiceLogService.GetServiceLogAsync(serviceLogId);
        if (sl != null)
        {
            sl.UnitDetails = sl.UnitDetails ?? new();

            var parameters = new DialogParameters<ServiceLogDialog> { { x => x.Model, sl } };

            var dialog = await DialogService.ShowAsync<ServiceLogDialog>("Edit Service Log", parameters);
            var result = await dialog.Result;
            if (!result.Canceled && (bool)result.Data)
            {
                Snackbar.Add($"Service Log successfully updated", Severity.Success);
                await OnInitializedAsync();
            }
        }
        else
        {
            Snackbar.Add($"Oops! An error has occurred. This service log is not in the database.", Severity.Error);
        }
    }
    private async Task RemoveServiceLog(int serviceLog)
    {
        var options = new DialogOptions
        {
            BackdropClick = false,
            MaxWidth = MaxWidth.Small,
            Position = DialogPosition.Center,
        };
        var dialog = await DialogService.ShowAsync<DeleteDialog>("Delete Service Log", options);
        var result = await dialog.Result;
        if (!result.Canceled && (bool)result.Data)
        {
            var delete = await ServiceLogService.DeleteServiceLogAsync(serviceLog);
            if (delete)
            {
                Snackbar.Add($"Service Log successfully deleted", Severity.Success);
                await OnInitializedAsync();
            }
            else
            {
                Snackbar.Add($"Oops! An error has occurred. This service log is not in the database.", Severity.Error);
            }
        }
    }
}
