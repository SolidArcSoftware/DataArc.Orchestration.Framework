using Demo.Application.Features.Auth.UserImports.Services;

namespace Demo.WebApi.Endpoints.Auth
{
    public static class AuthEndpointExtensions
    {
        public static IEndpointRouteBuilder MapAuthEndpoints(
            this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPost(
                "/api/auth/imports",
                async (IUserImportsService userImportsService) =>
                {
                    var response =
                        await userImportsService.ImportUserData();

                    return Results.Ok(response);
                })
                .WithName("ImportUserData")
                .WithTags("Auth");

            return endpoints;
        }
    }
}