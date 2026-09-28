using Demo.Persistence.DbContexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Persistence.Modules.IT
{
    public static class ITPersistenceRegistration
    {
        public static IServiceCollection AddITPersistence(
            this IServiceCollection services,
            ConfigurationManager configurationManager)
        {
            services.AddDbContextFactory<ItDbContext>(
                options =>
                    options.UseSqlServer(
                        configurationManager.GetConnectionString(
                            "DataArcDemoDb")),
                ServiceLifetime.Scoped);

            return services;
        }
    }
}