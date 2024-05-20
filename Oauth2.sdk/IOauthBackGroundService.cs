using Oauth2.sdk.Models;

namespace Oauth2.sdk
{
    public interface IOauthBackGroundService
    {
        AuthResponse? Tokens { get; set; }

        Task<string> GetAccessTokenAsync();
    }
}