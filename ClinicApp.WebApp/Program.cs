using ClinicApp.Infrastructure.Data;
using ClinicApp.WebApp.Interfaces;
using ClinicApp.WebApp.Services;
using MudBlazor;
using MudBlazor.Services;
using Oauth2.sdk.Models;
using Oauth2.sdk.DependencyInjection;
using ClinicApp.WebApp.Middlewares;
using ClinicApp.Infrastructure.Interfaces;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopRight;
    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 10000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});


builder.Logging.ClearProviders().AddConsole();
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));
builder.Services.Configure<CredentialsSettings>(builder.Configuration.GetSection("CredentialsSettings"));

builder.Services.AddScoped<NavigationService>();

// Add Services Injections
builder.Services.AddTransient<IClient, ClientService>();
builder.Services.AddTransient<IDiagnosis, DiagnosisService>();
builder.Services.AddTransient<IReleaseInformation, ReleaseInformationService>();
builder.Services.AddTransient<IPhysician, PhysicianService>();
builder.Services.AddTransient<IContractorType, ContractorTypeService>();
builder.Services.AddTransient<IProcedure, ProcedureService>();
builder.Services.AddTransient<IInsurance, InsuranceService>();
builder.Services.AddTransient<ICompany, CompanyService>();
builder.Services.AddTransient<ISecurityManagement, SecurityManagementService>();
builder.Services.AddTransient<IAppMenusService, AppMenusService>();
builder.Services.AddTransient<IReport, ReportService>();
builder.Services.AddTransient<IPeriod, PeriodService>();
builder.Services.AddTransient<IUsersService, UserService>();
builder.Services.AddTransient<IPayroll, PayrollService>();
builder.Services.AddTransient<IServiceLog, ServiceLogService>();
builder.Services.AddTransient<IPlaceOfService, PlaceOfServiceService>();

// Add convert to https middleware
builder.Services.AddTransient<ConvertToHttpsUriMiddleware>();

builder.Services.AddSecurityClientApplication(builder.Configuration);
builder.Services.AddHttpClient("CustomClient")
        .ConfigurePrimaryHttpMessageHandler(() =>
        {
            var handler = new HttpClientHandler();

#if DEBUG
            handler.ServerCertificateCustomValidationCallback =
                (message, cert, chain, sslPolicyErrors) => true;
#endif

            return handler;
        });

builder.Services.AddSession();

builder.Services.AddCors(options =>
{
    options.AddPolicy("default", policy =>
    {
        policy.SetIsOriginAllowed(allowedCors => true)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});


// Add open telemetry

// buscar como hacer batcher, para enviar los datos en bloques y configurar default trace provider, tracer propagator, processor
//builder.Services.AddOpenTelemetry().WithTracing(b =>
//{
//    b.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(builder.Environment.ApplicationName));
//    b.AddAspNetCoreInstrumentation();
//    b.AddHttpClientInstrumentation();
//    b.AddOtlpExporter(opts => { opts.Endpoint = new Uri("http://localhost:4317"); });
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();

// add middlewares
app.UseMiddleware<ConvertToHttpsUriMiddleware>();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();


#pragma warning disable ASP0014
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
