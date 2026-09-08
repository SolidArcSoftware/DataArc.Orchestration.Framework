namespace DataArc.Orchestration.Framework.Demo.Persistence.Database.Seeder
{
    public interface IDatabaseSeeder
    {
        Task<int> SeedDatabaseAsync();
    }
}