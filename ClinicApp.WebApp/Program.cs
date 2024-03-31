using ClinicApp.Infrastructure.Data;
using ClinicApp.WebApp.Data;
using ClinicApp.WebApp.Interfaces;
using ClinicApp.WebApp.Services;
using MudBlazor;
using MudBlazor.Services;
using System.Text.Json.Serialization;

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

builder.Services.AddHttpClient();

builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();

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
