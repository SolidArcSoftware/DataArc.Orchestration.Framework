using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using DataArc.Orchestration.Framework.Demo.Application.Features.HR;
using DataArc.Orchestration.Framework.Demo.Application.Modules.HR.Adapters;
using DataArc.Orchestration.Framework.Demo.Application.UseCases.HR;
using DataArc.Orchestration.Framework.Demo.Application.UseCases.HR.Ports;
using DataArc.Orchestration.Framework.Demo.Orchestration;
using DataArc.Orchestration.Framework.Demo.Persistence;
using DataArc.Orchestration.Framework.Demo.Application.Modules.HR.Repository;
using DataArc.Orchestration.Framework.Demo.Application.Features.HR.Repository;

namespace DataArc.Orchestration.Framework.Demo.Application.Modules
{
    public static class HRModuleRegistration
    {
        public static IServiceCollection AddHRModule(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            // Persistence
            services.AddPersistence(configurationManager);

            // HR Queries
            services.TryAddScoped<IHRRepository, HRRepository>();

            // HR Orchestration
            services.AddHROrchestration();

            // HR Orchestration Port & Adapters
            services.TryAddScoped<IHROrchestration, HROrchestrationAdapter>();

            // HR features
            services.AddHRFeatures();

            // HR Use Cases
            services.AddHRUseCases();

            return services;
        }
    }
}