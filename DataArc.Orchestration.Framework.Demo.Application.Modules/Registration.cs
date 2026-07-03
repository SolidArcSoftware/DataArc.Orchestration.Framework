using DataArc.Orchestration.Framework.Demo.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataArc.Orchestration.Framework.Demo.Application.Modules.Finance
{
    public static class FinanceModuleRegistration
    { 
        public static IServiceCollection AddFinanceModule(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            // Persistence
            services.AddPersistence(configurationManager);

            

            
            return services;
        }
    }
}
