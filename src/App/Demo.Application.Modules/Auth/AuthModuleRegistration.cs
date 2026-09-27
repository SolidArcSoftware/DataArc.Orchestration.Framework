using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Demo.Application.Features.HR.EmployeeImports.Services;
using Demo.Application.Modules.Auth.Adapters;
using Demo.Application.Modules.Auth.Services;
using Demo.Orchestration.Auth;
using Demo.Orchestration.Auth.Ports;
using Demo.Persistence.Modules.Auth;

namespace Demo.Application.Modules.Auth
{
    public static class AuthModuleRegistration
    {
        public static IServiceCollection AddIdentityModule(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            // Auth Persistence
            services.AddIdentityPersistence(configurationManager);

            // Auth Repositories
            

            // Auth Orchestration
            services.AddAuthOrchestration();

            // Auth Orchestration Port & Adapters
            services.AddScoped<IAuthOrchestrationPort, AuthOrchestrationAdapter>();

            // Auth Policies

            // HR features / services
            services.TryAddScoped<IUserImportsService, UserImportsService>();

            return services;
        }
    }
}