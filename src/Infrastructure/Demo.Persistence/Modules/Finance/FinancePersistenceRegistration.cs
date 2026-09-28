using Demo.Persistence.DbContexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Persistence.Modules.Finance
{
    public static class FinancePersistenceRegistration
    {
        public static IServiceCollection AddFinancePersistence(
            this IServiceCollection services,
            ConfigurationManager configurationManager)
        {
            services.AddDbContextFactory<FinanceDbContext>(
                options =>
                    options.UseSqlServer(
                        configurationManager.GetConnectionString(
                            "DataArcDemoDb")),
                ServiceLifetime.Scoped);

            return services;
        }
    }
}