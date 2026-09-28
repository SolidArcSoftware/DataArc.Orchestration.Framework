using Demo.Persistence.DbContexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Persistence.Modules.Operations
{
    public static class OperationsPersistenceRegistration
    {
        public static IServiceCollection AddOperationsPersistence(
            this IServiceCollection services,
            ConfigurationManager configurationManager)
        {
            services.AddDbContextFactory<OperationsDbContext>(
                options =>
                    options.UseSqlServer(
                        configurationManager.GetConnectionString(
                            "DataArcDemoDb")),
                ServiceLifetime.Scoped);

            return services;
        }
    }
}