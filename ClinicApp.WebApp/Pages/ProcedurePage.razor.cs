using ClinicApp.Core.Models;
using ClinicApp.WebApp.Components.Dialogs;
using ClinicApp.WebApp.Interfaces;
using ClinicApp.WebApp.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ClinicApp.WebApp.Pages;

public partial class ProcedurePage : ComponentBase
{
    [Inject] IDialogService DialogService { get; set; } = null!;
    [Inject] ISnackbar Snackbar { get; set; } = null!;
    [Inject] IProcedure ProcedureSevice { get; set; } = null!;
    public bool _loading = false;

    IEnumerable<Procedure> Procedures = new List<Procedure>();

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _loading = true;
            Procedures = await ProcedureSevice.GetProcedureAsync("");
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
    private async Task AddProcedure()
    {
        var procedure = new Procedure();
        procedure.ContractorType = new ContractorType();

        var parameters = new DialogParameters<ProcedureDialog> { { x => x.Model, procedure } };
        var dialog = await DialogService.ShowAsync<ProcedureDialog>("Add Procedure", parameters);
        var result = await dialog.Result;
        if (!result.Canceled && (bool)result.Data)
        {
            Snackbar.Add($"Procedure successfully created", Severity.Success);
            await OnInitializedAsync();
        }
    }

    private async Task EditProcedure(int procedureId)
    {
        var procedure = await ProcedureSevice.GetProcedureAsync(procedureId);
        if (procedure != null)
        {
            var parameters = new DialogParameters<ProcedureDialog> { { x => x.Model, procedure } };

            var dialog = await DialogService.ShowAsync<ProcedureDialog>("Edit Procedure", parameters);
            var result = await dialog.Result;
            if (!result.Canceled && (bool)result.Data)
            {
                Snackbar.Add($"Procedure successfully updated", Severity.Success);
                await OnInitializedAsync();
            }
        }
        else
        {
            Snackbar.Add($"Oops! An error has occurred. This period is not in the database.", Severity.Error);
        }
    }
    private async Task RemoveProcedure(int procedureId)
    {
        var options = new DialogOptions
        {
            DisableBackdropClick = false,
            MaxWidth = MaxWidth.Small,
            Position = DialogPosition.Center,
        };
        var dialog = await DialogService.ShowAsync<DeleteDialog>("Delete Procedure", options);
        var result = await dialog.Result;
        if (!result.Canceled && (bool)result.Data)
        {
            var delete = await ProcedureSevice.DeleteProcedureAsync(procedureId);
            if (delete)
            {
                Snackbar.Add($"Procedure successfully deleted", Severity.Success);
                await OnInitializedAsync();
            }
            else
            {
                Snackbar.Add($"Oops! An error has occurred. This Procedure is not in the database.", Severity.Error);
            }
        }
    }
}
