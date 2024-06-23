using ClinicApp.Core.Dtos;
using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Interfaces;
using ClinicApp.WebApp.Components.Dialogs;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Oauth2.sdk.Models;

namespace ClinicApp.WebApp.Pages;

public partial class UserPage : ComponentBase
{
    [Inject] IDialogService DialogService { get; set; } = null!;
    [Inject] ISnackbar Snackbar { get; set; } = null!;
    [Inject] IUsersService UserSevice { get; set; } = null!;
    public bool _loading = false;

    IEnumerable<UserVM> Users = new List<UserVM>();

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _loading = true;
            Users = await UserSevice.GetAllUsers();
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
    private async Task AddUser()
    {
        var user = new UserVM();
        user.Roles = new List<Oauth2.sdk.Models.Role>();

        var parameters = new DialogParameters<UserDialog> { { x => x.Model, user } };
        var dialog = await DialogService.ShowAsync<UserDialog>("Add User", parameters);
        var result = await dialog.Result;
        if (!result.Canceled && (bool)result.Data)
        {
            Snackbar.Add($"User successfully created", Severity.Success);
            await OnInitializedAsync();
        }
    }

    private async Task EditUser(string userId)
    {
        var user = await UserSevice.GetUser(userId);
        if (user != null)
        {
            var parameters = new DialogParameters<UserDialog> { { x => x.Model, user } };

            var dialog = await DialogService.ShowAsync<UserDialog>("Edit User", parameters);
            var result = await dialog.Result;
            if (!result.Canceled && (bool)result.Data)
            {
                Snackbar.Add($"User successfully updated", Severity.Success);
                await OnInitializedAsync();
            }
        }
        else
        {
            Snackbar.Add($"Oops! An error has occurred. This user is not in the database.", Severity.Error);
        }
    }
    private async Task RemoveUser(string userId)
    {
        var options = new DialogOptions
        {
            DisableBackdropClick = false,
            MaxWidth = MaxWidth.Small,
            Position = DialogPosition.Center,
        };
        var dialog = await DialogService.ShowAsync<DeleteDialog>("Delete User", options);
        var result = await dialog.Result;
        if (!result.Canceled && (bool)result.Data)
        {
            var delete = await UserSevice.DeleteUser(userId);
            if (delete)
            {
                Snackbar.Add($"User successfully deleted", Severity.Success);
                await OnInitializedAsync();
            }
            else
            {
                Snackbar.Add($"Oops! An error has occurred. This User is not in the database.", Severity.Error);
            }
        }
    }
}
