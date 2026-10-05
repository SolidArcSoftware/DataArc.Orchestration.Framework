using Demo.Persistence.DbContexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Persistence.Modules.HR
{
    public static class HRPersistenceRegistration
    {
        public static IServiceCollection AddHrPersistence(
            this IServiceCollection services,
            ConfigurationManager configurationManager)
        {
            services.AddDbContextFactory<HrDbContext>(
                options =>
                    options.UseSqlServer(
                        configurationManager.GetConnectionString(
                            "DataArcDemoDb")),
                ServiceLifetime.Scoped);

            return services;
        }
    }
}