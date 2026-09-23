using Demo.Persistence.Modules.Finance;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Application.Modules.Finance
{
    public static class FinanceModuleRegistration
    {
        public static IServiceCollection AddFinanceModule(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            // Persistence
            services.AddFinancePersistence(configurationManager);

            return services;
        }
    }
}