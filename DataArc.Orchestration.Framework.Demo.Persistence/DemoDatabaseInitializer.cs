using Microsoft.Extensions.DependencyInjection;
using DataArc.Orchestration.Framework.Demo.Persistence.Database.Seeder;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataArc.Orchestration.Framework.Demo.Persistence
{
    public static class DemoDatabaseInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var databaseCreator = serviceProvider.GetRequiredService<IDatabaseCreator>();
            var databaseSeeder = serviceProvider.GetRequiredService<IDatabaseSeeder>();

            await ResetDatabasesAsync(
                databaseCreator,
                databaseSeeder);
        }

        private static async Task ResetDatabasesAsync(
            IDatabaseCreator databaseCreator,
            IDatabaseSeeder databaseSeeder) 
        {

            if (!databaseCreator.EnsureDeleted())
                throw new InvalidOperationException($"Failed to delete database using {databaseCreator.GetType().Name}.");

            Console.WriteLine("Database deleted successfully.");

            if (!databaseCreator.EnsureCreated())
                throw new InvalidOperationException($"Failed to create database using {databaseCreator.GetType().Name}.");

            Console.WriteLine("Database created successfully.");

            var rowsAffected = await databaseSeeder.SeedDatabaseAsync();
            if (rowsAffected == 0) {
                throw new InvalidOperationException($"Seed database command failed");
            }                

            Console.WriteLine($"Database seeded successfully. ({rowsAffected}) Records affected");
        }
    }
}