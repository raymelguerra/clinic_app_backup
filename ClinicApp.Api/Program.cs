using ClinicApp.Api.DependencyInjection;
using ClinicApp.Api.Interfaces;
using ClinicApp.Api.MappingProfiles;
using ClinicApp.Api.Middlewares;
using ClinicApp.Api.Services;
using ClinicApp.Core.Models;
using ClinicApp.Infrastructure.Data;
using ClinicApp.Infrastructure.Interfaces;
using ClinicApp.Infrastructure.Persistence;
using ClinicApp.Infrastructure.Persistence.Configurations;
using ClinicApp.Reports.Interfaces;
using ClinicApp.Reports.Services;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// OData configuration
builder.Services.AddControllers().AddOData(opt =>
{
    opt.Select().Filter().OrderBy().Count().SkipToken();
});

var contextconfig = new ContextConfiguration();
builder.Configuration
.Bind("ContextConfiguration", contextconfig);

builder.Services.AddDbContext<InsuranceContext>(config =>
{
    config.UseNpgsql(contextconfig.Insurance_ConnectionString);
});

// add automapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Security configuration
builder.Services.AddOpenApiEntries();
builder.Services.AddSecurityApplication(builder.Configuration);

builder.Services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true).AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);


//Add menus configuration
builder.Configuration
    .AddJsonFile("menu-approles.json", optional: false, reloadOnChange: true);
builder.Services.Configure<MenuConfiguration>(
    builder.Configuration.GetSection("MenuConfiguration"));

// Add services to the container.
builder.Services.AddScoped<IMenusService, MenusService>();
builder.Services.AddScoped<IReportsFR, ReportsFRServices>();
builder.Services.AddTransient<IDbInitialize, DbInitializer>();
builder.Services.AddTransient<IUsersService, UsersService>();

// Add OPen telemetry
builder.Services.AddOpenTelemetry().WithTracing( b => {
    b.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(builder.Environment.ApplicationName))
    .AddAspNetCoreInstrumentation()
    .AddOtlpExporter(opts => { opts.Endpoint = new Uri("http://localhost:4317"); });
});

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitialize>();
    dbInitializer.Initialize();
}

app.UseSwagger()
    .UseSwaggerUI(c =>
    {
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        //c.SwaggerEndpoint("/swagger/v2/swagger.json", "v2");
        c.RoutePrefix = string.Empty;
    });


app.UseMiddleware<CustomExceptionMiddleware>();
// Configurar Middleware para Logging
app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An unhandled exception occurred.");
        throw;
    }
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
