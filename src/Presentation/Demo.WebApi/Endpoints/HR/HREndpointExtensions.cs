using Demo.Application.Features.HR.EmployeeOnboarding.Dtos;
using Demo.Application.Features.HR.EmployeeOnboarding.Services;

using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApi.Endpoints.HR
{
    public static class HREndpointExtensions
    {
        public static IEndpointRouteBuilder MapHREndpoints(
            this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost(
                "/api/hr/onboarding",
                async (
                    [FromServices] IEmployeeOnboardingService employeeOnboardingService,
                    [FromBody] OnboardEmployeeRequestDto onboardEmployeeRequest) =>
                {
                    var response =
                        await employeeOnboardingService.OnboardEmployeeAsync(
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

            return endpoints;
        }
    }
}