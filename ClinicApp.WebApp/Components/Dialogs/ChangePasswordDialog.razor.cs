using ClinicApp.Infrastructure.Dto;
using ClinicApp.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Oauth2.sdk;
using System.Text.RegularExpressions;
namespace ClinicApp.WebApp.Components.Dialogs;

public partial class ChangePasswordDialog : ComponentBase
{
    [Inject] ISnackbar Snackbar { get; set; } = null!;
    [Inject] IUsersService UsersService { get; set; } = null!;
    [Inject] IUserManagementService UserManagementService { get; set; } = null!;
    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = null!;


    private MudForm? form;
    MudTextField<string> pwField = null!;

    private IEnumerable<string> PasswordStrength(string pw)
    {
        if (string.IsNullOrWhiteSpace(pw))
        {
            yield return "Password is required!";
            yield break;
        }
        if (pw.Length < 8)
            yield return "Password must be at least of length 8";
        if (!Regex.IsMatch(pw, @"[A-Z]"))
            yield return "Password must contain at least one capital letter";
        if (!Regex.IsMatch(pw, @"[a-z]"))
            yield return "Password must contain at least one lowercase letter";
        if (!Regex.IsMatch(pw, @"[0-9]"))
            yield return "Password must contain at least one digit";
        if (!Regex.IsMatch(pw, @"[!@#$%^&*()_+}{:|>?<]"))
            yield return "Password must contain at least one special character";
    }

    private string PasswordMatch(string arg)
    {
        if (pwField.Value != arg)
            return "Passwords don't match";
        return null;
    }

    async void Submit()
    {
        await form!.Validate();
        if (!form.IsValid) return;

        try
        {
            var userId = UserManagementService.GetUserId()!;
            ChangePasswordDto pass = new() { NewPassword = pwField.Value };
            if (await UsersService.ChangePassword(userId, pass))
                MudDialog!.Close(DialogResult.Ok(true));
            else
                Snackbar!.Add($"Oops, there was an error changing the password. Please check that this password has not been used before.", Severity.Error);
        }
        catch (Exception ex)
        {
            Snackbar!.Add($"Oops, an error occurred. The error type is: {ex.Message}.", Severity.Error);
        }
    }
    void Cancel() => MudDialog.Cancel();

}
