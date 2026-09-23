using Demo.Persistence.Modules.Operations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Application.Modules.Operations
{
    public static class OperationsModuleRegistration
    {
        public static IServiceCollection AddOperationsModule(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            services.AddOperationsPersistence(configurationManager);

            //Repositories
            //Orchestration
            //Orchestration Port & Adapters
            //Policies
            //features / services

            return services;
        }
    }
}