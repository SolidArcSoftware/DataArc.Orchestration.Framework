using DataArc.EntityFrameworkCore;

using Demo.Application.Modules.Auth;
using Demo.Application.Modules.Finance;
using Demo.Application.Modules.HR;
using Demo.Application.Modules.IT;
using Demo.Application.Modules.Operations;
using Demo.Persistence.Database;
using Demo.Persistence.Database.Seeder;
using Demo.WebApi.Endpoints.Auth;
using Demo.WebApi.Endpoints.HR;
using Demo.WebApi.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Aspire service discovery, health checks, telemetry and resilience.
builder.AddServiceDefaults();

// API error handling.
builder.Services.AddProblemDetails();

// Swagger / OpenAPI.
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SchemaFilter<OnboardEmployeeRequestExampleSchemaFilter>();
});

// DataArc.
builder.Services.AddDataArcCore();

// Application modules.
builder.Services.AddIdentityModule(builder.Configuration);
builder.Services.AddHRModule(builder.Configuration);
builder.Services.AddFinanceModule(builder.Configuration);
builder.Services.AddITModule(builder.Configuration);
builder.Services.AddOperationsModule(builder.Configuration);

builder.Services.AddSeederRegistration();

var app = builder.Build();

/*
 * Seed the reference data required by the demo.
 *
 * The database schema must already exist from the supplied
 * setup script before the application is started.
 */
using (var scope = app.Services.CreateScope())
{
    var databaseSeeder =
        scope.ServiceProvider.GetRequiredService<IDatabaseSeeder>();

    await databaseSeeder.SeedDatabaseAsync();
}

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

/*
 * Demo API endpoints.
 */
app.MapAuthEndpoints();
app.MapHREndpoints();

// Aspire health / liveness endpoints.
app.MapDefaultEndpoints();

app.Run();