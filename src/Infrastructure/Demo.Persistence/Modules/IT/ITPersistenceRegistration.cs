using Demo.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Demo.Persistence.Modules.IT
{
    public static class ITPersistenceRegistration
    {
        static ILoggerFactory factory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });


        public static IServiceCollection AddITPersistence(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            services.AddDbContextFactory<ItDbContext>(
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