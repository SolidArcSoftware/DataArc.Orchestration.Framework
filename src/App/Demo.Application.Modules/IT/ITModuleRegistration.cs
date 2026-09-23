using Demo.Persistence.Modules.IT;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Application.Modules.IT
{
    public static class ITModuleRegistration
    {
        public static IServiceCollection AddITModule(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            services.AddITPersistence(configurationManager);

            return services;
        }
    }
}