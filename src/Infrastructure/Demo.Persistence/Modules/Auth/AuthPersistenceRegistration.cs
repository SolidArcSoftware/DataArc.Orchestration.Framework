using Demo.Persistence.DbContexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Persistence.Modules.Auth
{
    public static class AuthPersistenceRegistration
    {
        public static IServiceCollection AddIdentityPersistence(
            this IServiceCollection services,
            ConfigurationManager configurationManager)
        {
            return services.AddDbContextFactory<AuthDbContext>(
                options =>
                    options.UseSqlServer(
                        configurationManager.GetConnectionString(
                            "DataArcDemoDb")),
                ServiceLifetime.Scoped);
        }
    }
}