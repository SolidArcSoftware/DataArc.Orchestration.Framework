using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Demo.Application.Domain.HR.Policies;
using Demo.Application.Features.HR.EmployeeOnboarding.Services;

using Demo.Application.Modules.Modules.HR.Adapters;
using Demo.Persistence.Modules.HR;

using Demo.Orchestration.HR;
using Demo.Orchestration.HR.Ports;

using Demo.Application.Modules.HR.Services;

namespace Demo.Application.Modules.HR
{
    public static class HRModuleRegistration
    {
        public static IServiceCollection AddHRModule(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            //HR Logging
            services.AddLogging();
            //HR Persistence
            services.AddHrPersistence(configurationManager);
            // HR Orchestration
            services.AddHROrchestration();
            // HR Orchestration Port & Adapters
            services.AddScoped<IHROrchestrationPort, HROrchestrationAdapter>();
            // HR Policies
            services.AddScoped<IEmployeeOnboardingPolicy, OnboardEmployeePolicy>();
            // HR features / services
            services.TryAddScoped<IEmployeeOnboardingService, EmployeeOnboardingService>();

            return services;
        }
    }
}