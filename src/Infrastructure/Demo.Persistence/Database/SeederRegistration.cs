using Demo.Persistence.Database.Seeder;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Demo.Persistence.Database
{
    public static class SeederRegistration
    {
        public static IServiceCollection AddSeederRegistration(this IServiceCollection services)
        {
            services.TryAddTransient<IDatabaseSeeder, DatabaseSeeder>();
            return services;
        }
    }
}