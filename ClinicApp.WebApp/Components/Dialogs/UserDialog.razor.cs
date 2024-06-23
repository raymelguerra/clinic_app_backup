using ClinicApp.Core.Dtos;
using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Data;
using ClinicApp.Infrastructure.Interfaces;
using ClinicApp.WebApp.Interfaces;
using ClinicApp.WebApp.Services.Validations;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using MudBlazor;
using Oauth2.sdk;
using Oauth2.sdk.Models;
namespace ClinicApp.WebApp.Components.Dialogs;

public partial class UserDialog : ComponentBase
{
    [Parameter]
    public UserVM Model { get; set; } = null!;
    [Inject] ISnackbar Snackbar { get; set; } = null!;
    [Inject] IUsersService UsersService { get; set; } = null!;
    [Inject] IRolesManagementService RolesManagementService { get; set; } = null!;
    [Inject] IOptions<CredentialsSettings> credentials { get; set; } = null!;
    [CascadingParameter] MudDialogInstance MudDialog { get; set; } = null!;

    private MudForm? form;
    private UserValidator userValidator = new();

    private IEnumerable<Oauth2.sdk.Models.Role>? _roles = new List<Oauth2.sdk.Models.Role>();
    Func<Oauth2.sdk.Models.Role, string> converter = p => p?.Name;


    protected async override Task OnParametersSetAsync()
    {
        _roles = await RolesManagementService.GetListClientRoles(credentials.Value.ClientApiId!);
    }

    async void Submit()
    {
        await form!.Validate();
        if (!form.IsValid) return;

        try
        {
            var result = MudDialog!.Title.Contains("Add") ? await UsersService.CreateUser(Model) : await UsersService!.UpdateUser(Model.Id!, Model);
            if (result)
                MudDialog!.Close(DialogResult.Ok(true));
            else
                Snackbar!.Add($"Oops, there was an error adding a new User.", Severity.Error);
        }
        catch (Exception ex)
        {
            Snackbar!.Add($"Oops, an error occurred. The error type is: {ex.Message}.", Severity.Error);
        }
    }
    void Cancel() => MudDialog.Cancel();

}
