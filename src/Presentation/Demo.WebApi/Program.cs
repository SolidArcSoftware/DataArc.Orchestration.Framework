using DataArc.EntityFrameworkCore;

using Demo.Application.Features.HR.EmployeeImports.Services;
using Demo.Application.Features.HR.EmployeeOnboarding.Dtos;
using Demo.Application.Features.HR.EmployeeOnboarding.Services;

using Demo.Application.Modules.Auth;
using Demo.Application.Modules.Finance;
using Demo.Application.Modules.HR;
using Demo.Application.Modules.IT;
using Demo.Application.Modules.Operations;

using Demo.WebApi.Swagger;

using Microsoft.AspNetCore.Mvc;

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

var app = builder.Build();

app.UseExceptionHandler();

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
        return Results.Problem(
            detail: response.FailureReason,
            statusCode: StatusCodes.Status422UnprocessableEntity);
    }

    return Results.Ok(response);
})
.WithName("OnboardEmployee")
.WithTags("HR");

// Aspire health / liveness endpoints.
app.MapDefaultEndpoints();

app.Run();