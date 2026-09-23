using Demo.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Demo.Persistence.Modules.Auth
{
    public static class AuthPersistenceRegistration
    {
        static ILoggerFactory factory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });

        public static IServiceCollection AddIdentityPersistence(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            services.AddDbContextFactory<AuthDbContext>(
                options =>
                    options
                        .UseSqlServer(
                            configurationManager.GetConnectionString("DataArcDemoDb"))
                        .UseLoggerFactory(factory),
                ServiceLifetime.Scoped);

            return services;
        }
    }
}