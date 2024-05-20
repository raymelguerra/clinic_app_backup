using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Oauth2.sdk.Models;
using System.Text.Json;

namespace Oauth2.sdk.Services
{
    public class OauthBackGroundService : BackgroundService, IOauthBackGroundService
    {
        public AuthResponse? Tokens { get; set; }

        private readonly IHttpClientFactory httpClientFactory;
        private readonly CredentialsSettings credentials;

        public OauthBackGroundService(
            IHttpClientFactory _httpClientFactory,
            IOptions<CredentialsSettings> options
        )
        {
            httpClientFactory = _httpClientFactory;
            credentials = options.Value;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (stoppingToken.IsCancellationRequested)
            {
                Tokens = await GetTokenAsync();
                Thread.Sleep((Tokens.Expires_in * 1000) - 50);
            }
        }

        public async Task<string> GetAccessTokenAsync()
        {
            if (Tokens?.Access_token is null)
                Tokens = await GetTokenAsync();

            return Tokens.Access_token!;
        }

        private async Task<AuthResponse> GetTokenAsync()
        {
            using var client = httpClientFactory.CreateClient();
            var requestContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", credentials.ClientId!),
                new KeyValuePair<string, string>("client_secret", credentials.ClientSecret!),
                new KeyValuePair<string, string>("username", credentials.UserName!),
                new KeyValuePair<string, string>("password", credentials.Password!),
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
    }
}
