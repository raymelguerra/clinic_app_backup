using ClinicApp.Infrastructure.Dto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace Ipcs.WebApp.Pages
{
    public class LoginModel : PageModel
    {

        public LoginModel()
        {
            
        }

        public async Task OnGet(string redirectUri)
        {
            await HttpContext.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties
            {
                RedirectUri = redirectUri?? "/"
            });
        }
    }
}
