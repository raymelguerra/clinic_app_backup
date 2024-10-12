using ClinicApp.Core.Models;
using ClinicApp.WebApp.Components.Dialogs;
using ClinicApp.WebApp.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ClinicApp.WebApp.Pages;

public partial class ClientPage : ComponentBase
{
    [Inject]
    private IClient ClientService { get; set; } = null!;
    [Inject]
    private ISnackbar Snackbar { get; set; }
    [Inject] IDialogService DialogService { get; set; }

    public bool _loading = false;

    IEnumerable<Client> Clients = new List<Client>();

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _loading = true;
            Clients = await ClientService.GetClientAsync("");
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
    private async Task AddClient()
    {
        Client cl = new();
        cl.Agreements = new();

        var parameters = new DialogParameters<PatientDialog> { { x => x.Model, cl } };

        var dialog = await DialogService.ShowAsync<PatientDialog>("Add Patient", parameters);
        var result = await dialog.Result;
        if (!result.Canceled && (bool)result.Data)
        {
            Snackbar.Add($"Patient successfully created", Severity.Success);
            await OnInitializedAsync();
        }
    }

    private async Task EditClient(int ClientId)
    {
        var cl = await ClientService.GetClientAsync(ClientId);
        if (cl != null)
        {
            cl.Agreements = cl.Agreements ?? new();

            var parameters = new DialogParameters<PatientDialog> { { x => x.Model, cl } };

            var dialog = await DialogService.ShowAsync<PatientDialog>("Edit Patient", parameters);
            var result = await dialog.Result;
            if (!result.Canceled && (bool)result.Data)
            {
                Snackbar.Add($"Patient successfully updated", Severity.Success);
                await OnInitializedAsync();
            }
        }
        else
        {
            Snackbar.Add($"Oops! An error has occurred. This patient is not in the database.", Severity.Error);
        }
    }

    private async Task RemoveClient(int ClientId)
    {
        var options = new DialogOptions
        {
            BackdropClick = false,
            MaxWidth = MaxWidth.Small,
            Position = DialogPosition.Center,
        };
        var dialog = await DialogService.ShowAsync<DeleteDialog>("Delete Patient", options);
        var result = await dialog.Result;
        if (!result.Canceled && (bool)result.Data)
        {
            var delete = await ClientService.DeleteClientAsync(ClientId);
            if (delete)
            {
                Snackbar.Add($"Patient successfully deleted", Severity.Success);
                await OnInitializedAsync();
            }
            else
            {
                Snackbar.Add($"Oops! An error has occurred. This patient is not in the database.", Severity.Error);
            }
        }
    }
}
