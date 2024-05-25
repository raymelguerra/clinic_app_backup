using Oauth2.sdk.DependencyInjection;
using Oauth2.sdk.Models;

namespace ClinicApp.Api.DependencyInjection
{
    public static class SecurityEntries
    {
        public static IServiceCollection AddSecurityApplication(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services
                .AddCors(options =>
                {
                    options.AddPolicy("default", policy =>
                    {
                        policy.SetIsOriginAllowed(allowedCors => true)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                    });
                });

            services.Configure<CredentialsSettings>(configuration.GetSection("CredentialsSettings"));
            services.AddSecurityResourcesServer(configuration);

            // convert to string this result configuration.GetSection("CredentialsSettings")
            Console.WriteLine(configuration.GetSection("CredentialsSettings").Get<CredentialsSettings>());
            return services;
        }
    }
}
