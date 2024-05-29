using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Oauth2.sdk.Models;
using System.Security.Claims;

namespace Oauth2.sdk.DependencyInjection
{
    public static class IdpMapping
    {
        public static void MapKeyCloakRolesToRoleClaims(UserInformationReceivedContext context, CredentialsSettings credentials)
        {
            if (context.Principal?.Identity is not ClaimsIdentity claimsIdentity) return;

            if (context.User.RootElement.TryGetProperty("preferred_username", out var username))
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, username.ToString()));
            }

            if (context.User.RootElement.TryGetProperty("realm_access", out var realmAccess)
                && realmAccess.TryGetProperty("roles", out var globalRoles))
            {
                foreach (var role in globalRoles.EnumerateArray())
                {
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role.ToString()));
                }
            }

            if (context.User.RootElement.TryGetProperty("resource_access", out var clientAccess)
                && clientAccess.TryGetProperty(credentials.ClientApiId!, out var client)
                && client.TryGetProperty("roles", out var clientRoles))
            {
                foreach (var role in clientRoles.EnumerateArray())
                {
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role.ToString()));
                }
            }
        }
    }
}
