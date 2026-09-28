using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Demo.Persistence.Modules.IT;

namespace Demo.Application.Modules.IT
{
    public static class ITModuleRegistration
    {
        public static IServiceCollection AddITModule(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            // IT Persistence
            services.AddITPersistence(configurationManager);

            return services;
        }
    }
}