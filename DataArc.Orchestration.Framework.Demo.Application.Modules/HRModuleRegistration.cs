using DataArc.Orchestration.Framework.Demo.Application.Domain.HR.Policies;
using DataArc.Orchestration.Framework.Demo.Application.Features.HR.EmployeeImports.Services;
using DataArc.Orchestration.Framework.Demo.Application.Features.HR.EmployeeOnboarding.Services;
using DataArc.Orchestration.Framework.Demo.Application.Features.HR.Repositories;
using DataArc.Orchestration.Framework.Demo.Application.Modules.HR.Adapters;
using DataArc.Orchestration.Framework.Demo.Application.Modules.HR.Repositories;
using DataArc.Orchestration.Framework.Demo.Application.Modules.HR.Services;
using DataArc.Orchestration.Framework.Demo.Orchestration;
using DataArc.Orchestration.Framework.Demo.Orchestration.HR.Ports;
using DataArc.Orchestration.Framework.Demo.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DataArc.Orchestration.Framework.Demo.Application.Modules
{
    public static class HRModuleRegistration
    {
        public static IServiceCollection AddHRModule(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            // Persistence
            services.AddPersistence(configurationManager);

            // Repositories
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