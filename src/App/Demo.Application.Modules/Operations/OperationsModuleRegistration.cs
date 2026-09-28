using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Demo.Persistence.Modules.Operations;

namespace Demo.Application.Modules.Operations
{
    public static class OperationsModuleRegistration
    {
        public static IServiceCollection AddOperationsModule(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            // Operations Persistence
            services.AddOperationsPersistence(configurationManager);

            return services;
        }
    }
}