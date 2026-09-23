using Demo.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Demo.Persistence.Modules.Operations
{
    public static class OperationsPersistenceRegistration
    {
        static ILoggerFactory factory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });

        public static IServiceCollection AddOperationsPersistence(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            services.AddDbContextFactory<OperationsDbContext>(
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