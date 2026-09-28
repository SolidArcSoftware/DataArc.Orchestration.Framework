using Microsoft.EntityFrameworkCore;

using DataArc.EntityFrameworkCore;
using DataArc.Orchestrator;

using Demo.Persistence.DbContexts;
using Demo.Persistence.Utils;

using Demo.Orchestration.Auth.Orchestration.Input;
using Demo.Orchestration.Auth.Orchestration.Output;

namespace Demo.Orchestration.Auth.Orchestration
{
    public sealed class ImportUsersDataOrchestrator
        : Orchestrator<ImportUsersInput, ImportUsersOutput>
    {
        private readonly IDbContextFactory<AuthDbContext> _authDbContextFactory;

        public ImportUsersDataOrchestrator(
            IDbContextFactory<AuthDbContext> authDbContextFactory)
        {
            _authDbContextFactory = authDbContextFactory;
        }

        public override async Task<ImportUsersOutput> ExecuteAsync(
            ImportUsersInput input,
            ImportUsersOutput output)
        {
            await using var dbContext =
                await _authDbContextFactory.CreateDbContextAsync();

            var importData = SeedDataGenerator
                .GenerateIdentityUserSeedData(input.ImportCountCount);

            await dbContext.AddBulkAsync(
                importData,
                input.ImportBatchSize);

            output.TotalRecordsProcessed = importData.Count;

            return output;
        }
    }
}