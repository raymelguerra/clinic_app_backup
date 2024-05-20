using Microsoft.Extensions.Options;
using Oauth2.sdk.Models;
using System.Text.Json;

namespace Oauth2.sdk.Services
{
    public class OauthService : IOauthService, IDisposable
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly CredentialsSettings credentials;

        public OauthService(
            IHttpClientFactory _httpClientFactory,
            IOptions<CredentialsSettings> options
        )
        {
            httpClientFactory = _httpClientFactory;
            credentials = options.Value;
        }

        public async Task<AuthResponse> GetTokenAsync(string userName, string password)
        {
            using var client = httpClientFactory.CreateClient(); 
            var requestContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", credentials.ClientId!),
                new KeyValuePair<string, string>("client_secret", credentials.ClientSecret!),
                new KeyValuePair<string, string>("username", userName),
                new KeyValuePair<string, string>("password", password),
                new KeyValuePair<string, string>("grant_type", credentials.GrantType ?? "password"),
                new KeyValuePair<string, string>("scope", credentials.Scope ?? "openid")
            });

            var response = await client.PostAsync($"{credentials.Authority}/protocol/openid-connect/token", requestContent);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {response.StatusCode}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var authResponse = JsonSerializer.Deserialize<AuthResponse>(responseContent);

            return authResponse!;
        }

        public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
        {
            using var client = httpClientFactory.CreateClient(); 

            var requestContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", credentials.ClientId!),
                new KeyValuePair<string, string>("client_secret", credentials.ClientSecret!),
                new KeyValuePair<string, string>("refresh_token", refreshToken),
                new KeyValuePair<string, string>("grant_type", "refresh_token")
            });

            var response = await client.PostAsync($"{credentials.Authority}/protocol/openid-connect/token", requestContent);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error: {response.StatusCode}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var authResponse = JsonSerializer.Deserialize<AuthResponse>(responseContent);

            return authResponse!;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
