using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Oauth2.sdk.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace Oauth2.sdk.Services
{
    public class TokenRefreshMiddleware
    {
        private readonly RequestDelegate next;
        private readonly HttpClient client;
        private readonly CredentialsSettings credentials;

        public TokenRefreshMiddleware(
            RequestDelegate _next, 
            IHttpClientFactory _httpClientFactory, 
            IOptions<CredentialsSettings> _options
        )
        {
            next = _next;
            client = _httpClientFactory.CreateClient();
            credentials = _options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            var expClaim = context.User.FindFirst("exp");
            if (expClaim != null && long.TryParse(expClaim.Value, out long expireTime))
            {
                var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                if (expireTime < currentTime)
                {
                    var refreshToken = context.User.FindFirst("refresh_token")?.Value;
                    if (!string.IsNullOrEmpty(refreshToken))
                    {
                        var parameters = new Dictionary<string, string>
                    {
                        { "grant_type", "refresh_token" },
                        { "refresh_token", refreshToken },
                        { "client_id", credentials.ClientId! },
                        { "client_secret", credentials.ClientSecret! }
                    };

                        var response = await client.PostAsync(
                            $"{credentials.Authority}admin/realms/{credentials.Realm}/protocol/openid-connect/token", 
                            new FormUrlEncodedContent(parameters));

                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(content);

                            // Actualizar el token de acceso y refresco en el contexto del usuario
                            // Aquí necesitarás implementar la lógica para almacenar y actualizar los tokens en tu aplicación

                            // Asigna el nuevo token de acceso al contexto del usuario
                            var tokenHandler = new JwtSecurityTokenHandler();
                            var accessToken = tokenHandler.ReadJwtToken(tokenResponse?.AccessToken);
                            context.User = new ClaimsPrincipal(new ClaimsIdentity(accessToken.Claims, "Bearer"));
                        }
                    }
                }
            }

            await next(context);
        }
    }

    public static class TokenRefreshMiddlewareExtensions
    {
        public static IApplicationBuilder UseTokenRefreshMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenRefreshMiddleware>();
        }
    }

    public class TokenResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
