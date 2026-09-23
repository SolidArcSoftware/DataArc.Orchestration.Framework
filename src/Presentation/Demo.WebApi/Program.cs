using DataArc.EntityFrameworkCore;

using Demo.Application.Modules.Operations;
using Demo.WebApi.Swagger;

using Demo.Application.Modules.Auth;
using Demo.Application.Modules.Finance;
using Demo.Application.Modules.HR;
using Demo.Application.Modules.IT;

using Demo.Application.Features.HR.EmployeeImports.Services;
using Demo.Application.Features.HR.EmployeeOnboarding.Dtos;
using Demo.Application.Features.HR.EmployeeOnboarding.Services;

using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SchemaFilter<OnboardEmployeeRequestExampleSchemaFilter>();
});


// DataArc
builder.Services.AddDataArcCore();

// Modules
builder.Services.AddIdentityModule(builder.Configuration);
builder.Services.AddHRModule(builder.Configuration);
builder.Services.AddFinanceModule(builder.Configuration);
builder.Services.AddITModule(builder.Configuration);
builder.Services.AddOperationsModule(builder.Configuration);

var app = builder.Build();

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
        return Results.Problem(detail: response.FailureReason, 
            statusCode: StatusCodes.Status422UnprocessableEntity);
    }

    return Results.Ok(response);
})
.WithName("OnboardEmployee")
.WithTags("HR");

app.Run();