using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Oauth2.sdk.Models;
using Oauth2.sdk.Services.KeycloakProvider;
using System.Text;
using static Oauth2.sdk.DependencyInjection.IdpMapping;

namespace Oauth2.sdk.DependencyInjection
{
    //Add Credentials section to program:
    //services.Configure<CredentialsSettings>(Configuration.GetSection("CredentialsSettings"));
    public static class AddAuthDependencies
    {
        public static IServiceCollection AddSecurityClientApplication(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = _ => false;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddOpenIdConnect(options =>
            {
                var credentialsSettings =
                        configuration.GetSection("CredentialsSettings").Get<CredentialsSettings>();

                options.Authority = $"{credentialsSettings!.Authority}realms/{credentialsSettings.Realm}/";
                options.ClientId = credentialsSettings.ClientId;
                options.ClientSecret = credentialsSettings.ClientSecret;
                options.RequireHttpsMetadata = credentialsSettings.Authority!.StartsWith("https://");
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.SaveTokens = true;
                options.MapInboundClaims = true;
                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");
                options.Scope.Add("roles");
                options.Events = new OpenIdConnectEvents
                {
                    OnUserInformationReceived = context =>
                    {
                        MapKeyCloakRolesToRoleClaims(context, credentialsSettings);
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization();

            services.AddHttpContextAccessor();

            services.AddScoped<IUserManagementService, UserManagementService_KeycloakProvider>();
            services.AddScoped<IRolesManagementService, RolesManagementService_KeycloakProvider>();

            return services;
        }

        public static IServiceCollection AddSecurityResourcesServer(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
                {
                    var credentialsSettings =
                        configuration.GetSection("CredentialsSettings").Get<CredentialsSettings>();

                    options.Authority = $"{credentialsSettings!.Authority}realms/{credentialsSettings.Realm}/";
                    options.RequireHttpsMetadata = credentialsSettings.Authority!.StartsWith("https://");
                    options.Audience = credentialsSettings.Audience;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = $"{credentialsSettings.Authority}realms/{credentialsSettings.Realm}/",
                        ValidAudience = credentialsSettings.ClientId,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(credentialsSettings.ClientSecret!)),
                        ClockSkew = TimeSpan.Zero
                    };

                    options.BackchannelHttpHandler =
                        new HttpClientHandler { ServerCertificateCustomValidationCallback = delegate { return true; } };
                });

            services.AddHttpContextAccessor();

            services.AddScoped<IUserManagementService, UserManagementService_KeycloakProvider>();

            return services;
        }
    }
}
