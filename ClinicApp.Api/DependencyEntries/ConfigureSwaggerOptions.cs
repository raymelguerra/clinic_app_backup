using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ClinicApp.Api.DependencyInjection
{
    public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly string _uriString = "https://expanding.insurance.abaanalystgroup.live/";
        private readonly IApiVersionDescriptionProvider provider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                  description.GroupName,
                    new OpenApiInfo()
                    {
                        Title = $"CliniApp API {description.ApiVersion}",
                        Description = "ClinicApp Web Api",
                        Version = description.ApiVersion.ToString(),
                        Contact = new OpenApiContact
                        {
                            Name = "Aba Analyist group",
                            Url = new Uri(_uriString)
                        }
                    });
            }

            // Define el esquema de seguridad para JWT Bearer
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                  Enter 'Bearer' [space] and then your token in the text input below.
                  \r\n\r\nExample: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                  new OpenApiSecurityScheme
                  {
                    Reference = new OpenApiReference
                    {
                      Type = ReferenceType.SecurityScheme,
                      Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                  },
                  new List<string>()
                }
            });
        }
    }
}
