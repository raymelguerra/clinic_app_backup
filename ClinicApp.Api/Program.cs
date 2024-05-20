using ClinicApp.Api.Middlewares;
using ClinicApp.Infrastructure.Data;
using ClinicApp.Infrastructure.Interfaces;
using ClinicApp.Infrastructure.Persistence;
using Ipcs.Infrastructure.Persistence.Configurations;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// KeyCloak configuration
builder.Services.AddKeycloakAuthentication(builder.Configuration);
builder.Services.AddKeycloakAuthorization(builder.Configuration);

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

builder.Services.AddSwaggerGen(c =>
{
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Keycloak",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.OpenIdConnect,
        OpenIdConnectUrl = new Uri($"{builder.Configuration["Keycloak:auth-server-url"]}realms/{builder.Configuration["Keycloak:realm"]}/.well-known/openid-configuration"),
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = "Bearer",
            Type = ReferenceType.SecurityScheme
        }
    };
    c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });
});

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

app.UseCors(a => a.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
