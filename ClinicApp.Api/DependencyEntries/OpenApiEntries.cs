using Asp.Versioning;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace ClinicApp.Api.DependencyInjection
{
    public static class OpenApiEntries
    {
        public static IServiceCollection AddOpenApiEntries(
            this IServiceCollection services
        )
        {
            services
                .AddEndpointsApiExplorer()
                .AddApiVersioning(o =>
                {
                    o.AssumeDefaultVersionWhenUnspecified = true;
                    o.DefaultApiVersion = new ApiVersion(1, 0);
                    o.ReportApiVersions = true;
                    o.ApiVersionReader = ApiVersionReader.Combine(
                        new QueryStringApiVersionReader("api-version"),
                        new HeaderApiVersionReader("X-Version"),
                        new MediaTypeApiVersionReader("ver"));
                })
                .AddMvc() // ← bring in MVC (Core); not required for Minimal APIs
                .AddApiExplorer(
                    options =>
                    {
                        options.GroupNameFormat = "'v'VVV";
                        options.SubstituteApiVersionInUrl = true;

                    });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddSwaggerGen(c =>
            {
                c.DocInclusionPredicate((version, desc) =>
                {
                    if (!desc.TryGetMethodInfo(out MethodInfo methodInfo))
                    {
                        return false;
                    }
                    var versions = methodInfo.DeclaringType!.GetCustomAttributes()
                            .Where(attributeData => attributeData.GetType() == typeof(ApiVersionAttribute))
                            .Select(v => (ApiVersionAttribute)v);

                    return versions.Any(v => v.Versions.Any(v1 => $"v{v1.MajorVersion}" == version));
                });
                c.CustomSchemaIds(s => s.FullName!.Replace("+", "."));
                c.DescribeAllParametersInCamelCase();
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            return services;
        }
    }
}
