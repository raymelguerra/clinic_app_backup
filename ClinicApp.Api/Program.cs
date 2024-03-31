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

builder.Services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true).AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Add services to the container.
builder.Services.AddTransient<IDbInitialize, DbInitializer>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var contextconfig = new ContextConfiguration();
builder.Configuration
.Bind("ContextConfiguration", contextconfig);

builder.Services.AddDbContext<InsuranceContext>(config =>
{
    config.UseNpgsql(contextconfig.Insurance_ConnectionString);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitialize>();
    dbInitializer.Initialize();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CustomExceptionMiddleware>();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
