using DataArc.Orchestration.Framework.Demo.Application.Features.HR.EmployeeImports.Services;
using DataArc.Orchestration.Framework.Demo.Application.Features.HR.EmployeeOnboarding.Dtos;
using DataArc.Orchestration.Framework.Demo.Application.Features.HR.EmployeeOnboarding.Services;
using DataArc.Orchestration.Framework.Demo.Application.Modules;
using DataArc.Orchestration.Framework.Demo.Persistence;
using DataArc.Orchestration.Framework.Demo.WebApi.Swagger;

using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SchemaFilter<OnboardEmployeeRequestExampleSchemaFilter>();
});

// DataArc demo modules
builder.Services.AddHRModule(builder.Configuration);

var app = builder.Build();

// Demo database initialization
using (var scope = app.Services.CreateScope())
{
    await DemoDatabaseInitializer.InitializeAsync(scope.ServiceProvider);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/api/hr/imports", async (
    IEmployeeImportsService employeeImportsService) =>
{
    var response = await employeeImportsService.ImportEmployeeData();

    return Results.Ok(response);
})
.WithName("ImportHrData")
.WithTags("HR");

app.MapPost("/api/hr/onboarding", async (
    [FromServices] IEmployeeOnboardingService employeeOnboardingService,
    [FromBody] OnboardEmployeeRequestDto onboardEmployeeRequest) =>
{
    var response = await employeeOnboardingService.OnboardEmployeeAsync(
        onboardEmployeeRequest);

    if (!response.IsSuccess)
    {
        return Results.BadRequest(response);
    }

    return Results.Ok(response);
})
.WithName("OnboardEmployee")
.WithTags("HR");

app.Run();