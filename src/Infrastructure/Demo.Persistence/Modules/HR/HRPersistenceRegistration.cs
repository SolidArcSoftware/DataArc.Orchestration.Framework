using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Demo.Persistence.DbContexts;

namespace Demo.Persistence.Modules.HR
{
    public static class HRPersistenceRegistration
    {
        static ILoggerFactory factory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });

        public static IServiceCollection AddHrPersistence(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            services.AddDbContextFactory<HrDbContext>(
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