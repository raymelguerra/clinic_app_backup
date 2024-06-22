using ClinicApp.Core.Models;
using ClinicApp.WebApp.Components.Dialogs;
using ClinicApp.WebApp.Interfaces;
using ClinicApp.WebApp.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ClinicApp.WebApp.Pages;

public partial class PeriodPage : ComponentBase
{
    [Inject] IDialogService DialogService { get; set; } = null!;
    [Inject] ISnackbar Snackbar { get; set; } = null!;
    [Inject] IPeriod PeriodService { get; set; } = null!;
    public bool _loading = false;

    IEnumerable<Period> Periods = new List<Period>();

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _loading = true;
            Periods = await PeriodService.GetPeriodAsync("");
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
    private async Task AddPeriod()
    {
        var period = new Period();

        var parameters = new DialogParameters<PeriodDialog> { { x => x.Model, period } };
        var dialog = await DialogService.ShowAsync<PeriodDialog>("Add Period", parameters);
        var result = await dialog.Result;
        if (!result.Canceled && (bool)result.Data)
        {
            Snackbar.Add($"Period successfully created", Severity.Success);
            await OnInitializedAsync();
        }
    }

    private async Task EditPeriod(int periodId)
    {
        var period = await PeriodService.GetPeriodAsync(periodId);
        if (period != null)
        {
            var parameters = new DialogParameters<PeriodDialog> { { x => x.Model, period } };

            var dialog = await DialogService.ShowAsync<PeriodDialog>("Edit Period", parameters);
            var result = await dialog.Result;
            if (!result.Canceled && (bool)result.Data)
            {
                Snackbar.Add($"Period successfully updated", Severity.Success);
                await OnInitializedAsync();
            }
        }
        else
        {
            Snackbar.Add($"Oops! An error has occurred. This period is not in the database.", Severity.Error);
        }
    }
    private async Task RemovePeriod(int periodId)
    {
        var options = new DialogOptions
        {
            DisableBackdropClick = false,
            MaxWidth = MaxWidth.Small,
            Position = DialogPosition.Center,
        };
        var dialog = await DialogService.ShowAsync<DeleteDialog>("Delete Period", options);
        var result = await dialog.Result;
        if (!result.Canceled && (bool)result.Data)
        {
            var delete = await PeriodService.DeletePeriodAsync(periodId);
            if (delete)
            {
                Snackbar.Add($"Period successfully deleted", Severity.Success);
                await OnInitializedAsync();
            }
            else
            {
                Snackbar.Add($"Oops! An error has occurred. This Period is not in the database.", Severity.Error);
            }
        }
    }
}
