using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

using DataArc.Orchestration.Framework.Demo.Persistence.Database.Seeder;
using DataArc.Orchestration.Framework.Demo.Persistence.DbContexts;
using DataArc.Orchestration.Framework.Demo.Persistence.Database.Creator;

namespace DataArc.Orchestration.Framework.Demo.Persistence
{
    public static class PersistenceRegistration
    {
        static ILoggerFactory factory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });

        public static IServiceCollection AddPersistence(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            services.AddDbContextFactory<FinanceDbContext>(options =>
                options
                    .UseSqlServer(configurationManager.GetConnectionString("DataArcDemoDb"))
                    .UseLoggerFactory(factory));

            services.AddDbContextFactory<HrDbContext>(options =>
                options
                    .UseSqlServer(configurationManager.GetConnectionString("DataArcDemoDb"))
                    .UseLoggerFactory(factory));

            services.AddDbContextFactory<ItDbContext>(options =>
                options
                    .UseSqlServer(configurationManager.GetConnectionString("DataArcDemoDb"))
                    .UseLoggerFactory(factory));

            services.AddDbContextFactory<OperationsDbContext>(options =>
                options
                    .UseSqlServer(configurationManager.GetConnectionString("DataArcDemoDb"))
                    .UseLoggerFactory(factory));

            services.TryAddScoped<IDatabaseCreator, DatabaseCreator>();
            services.TryAddScoped<IDatabaseSeeder, DatabaseSeeder>();

            return services;
        }
    }
}