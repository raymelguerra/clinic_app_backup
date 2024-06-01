using Microsoft.AspNetCore.Components;
using MudBlazor;
using Oauth2.sdk;

namespace ClinicApp.WebApp.Components.ErrorPages
{
    public partial class Forbidden : ComponentBase
    {
        [Inject]
        private ISnackbar Snackbar { get; set; } = null!;

        [Inject]
        private IUserManagementService UserManagement { get; set; } = null!;

        private string? UserRol { get; set; }
        protected override void OnInitialized()
        {
            try
            {
                var roles = UserManagement.GetUserRoles();
                UserRol = roles.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Oops, an error occurred getting user roles. The error type is: {ex.Message}.", Severity.Error);
            }

        }

        private void GoBack()
        {
            Navigation.NavigateTo("/home");
        }
    }
}
