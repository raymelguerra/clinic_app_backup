using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Oauth2.sdk.Exceptions;
using Oauth2.sdk.Models;
using System.Net;

namespace Oauth2.sdk.Services.KeycloakProvider
{
    public class KeycloakProviderBase
    {
        protected readonly HttpClient client;
        protected readonly CredentialsSettings credentials;
        protected readonly IHttpContextAccessor context;

        public KeycloakProviderBase(
            IOptions<CredentialsSettings> _options,
            IHttpContextAccessor _httpContextAccessor
        )
        {
            client = new HttpClient();
            credentials = _options.Value;
            context = _httpContextAccessor;
        }

        public string? GetToken()
        {
            return context.HttpContext?
                .GetTokenAsync("access_token")
                .GetAwaiter()
                .GetResult();
        }

        protected async Task<string?> GetIdClient()
        {
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GetToken());

            var response = await client.GetAsync(
            $"{credentials.Authority}admin/realms/{credentials.Realm}/clients?clientId={credentials.ClientId}");

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();

            if (response.StatusCode == HttpStatusCode.Forbidden)
                throw new ForbiddenAccessException();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error: {response.StatusCode}");

            var clientList = JsonConvert.DeserializeObject<IEnumerable<Client>>(
                await response.Content.ReadAsStringAsync());

            return clientList?
                .FirstOrDefault()?.Id;
        }
    }
}