using ClinicApp.Api.DependencyInjection;
using ClinicApp.Api.Middlewares;
using ClinicApp.Infrastructure.Data;
using ClinicApp.Infrastructure.Interfaces;
using ClinicApp.Infrastructure.Persistence;
using Ipcs.Infrastructure.Persistence.Configurations;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

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

// Security configuration
builder.Services.AddOpenApiEntries();
builder.Services.AddSecurityApplication(builder.Configuration);

builder.Services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true).AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Add services to the container.
builder.Services.AddTransient<IDbInitialize, DbInitializer>();

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

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<CustomExceptionMiddleware>();

app.MapControllers();

app.Run();
