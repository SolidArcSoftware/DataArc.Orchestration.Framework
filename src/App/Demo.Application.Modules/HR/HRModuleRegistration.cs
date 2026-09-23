using Demo.Application.Domain.HR.Policies;
using Demo.Application.Features.HR.EmployeeImports.Services;
using Demo.Application.Features.HR.EmployeeOnboarding.Services;
using Demo.Application.Features.HR.Repositories;
using Demo.Application.Modules.Modules.HR.Adapters;
using Demo.Application.Modules.Modules.HR.Repositories;
using Demo.Application.Modules.Modules.HR.Services;
using Demo.Orchestration.HR;
using Demo.Orchestration.HR.Ports;
using Demo.Persistence.Modules.HR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Demo.Application.Modules.HR
{
    public static class HRModuleRegistration
    {
        public static IServiceCollection AddHRModule(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            //HR Persistence
            services.AddHrPersistence(configurationManager);

            //HR Repositories
            services.AddScoped<IHRRepository, HRRepository>();

            // HR Orchestration
            services.AddHROrchestration();

            // HR Orchestration Port & Adapters
            services.AddScoped<IHROrchestrationPort, HROrchestrationAdapter>();

            // HR Policies
            services.AddScoped<IEmployeeOnboardingPolicy, OnboardEmployeePolicy>();

            // HR features / services
            services.TryAddScoped<IEmployeeImportsService, EmployeeImportsService>();
            services.TryAddScoped<IEmployeeOnboardingService, EmployeeOnboardingService>();

            return services;
        }
    }
}