using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using DataArc.Orchestration.Framework.Demo.Application.Features.Finance;
using DataArc.Orchestration.Framework.Demo.Application.Modules.Finance.Adapters;
using DataArc.Orchestration.Framework.Demo.Application.UseCases.Finance;
using DataArc.Orchestration.Framework.Demo.Application.UseCases.Modules.Finance.Ports;
using DataArc.Orchestration.Framework.Demo.Orchestration;

namespace DataArc.Orchestration.Framework.Demo.Application.Modules.Finance
{
    public static class FinanceModuleRegistration
    { 
        public static IServiceCollection AddFinanceModule(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            // Finance Use Cases
            services.AddFinanceUseCases();

            // Finance features
            services.AddFinanceFeatures();

            // Finance Orchestration
            //services.AddFinanceOrchestration();

            // Finance Port & Adapters
            services.TryAddSingleton<IFinanceOrchestrationPort, FinanceOrchestrationAdapter>();

            return services;
        }
    }
}