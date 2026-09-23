using Demo.Persistence.Modules.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Application.Modules.Auth
{
    public static class AuthModuleRegistration
    {
        public static IServiceCollection AddIdentityModule(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            // Persistence
            services.AddIdentityPersistence(configurationManager);

            return services;
        }
    }
}