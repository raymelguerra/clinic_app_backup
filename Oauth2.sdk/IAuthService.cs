using Oauth2.sdk.Models;

namespace Oauth2.sdk
{
    public interface IOauthService
    {
        Task<AuthResponse> GetTokenAsync(string userName, string password);
        Task<AuthResponse> RefreshTokenAsync(string refreshToken);
    }
}