
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
        private readonly IDbContextFactory<HrDbContext> _hrDbContextFactory;

        public ImportUsersDataOrchestrator(
            IDbContextFactory<HrDbContext> hrDbContextFactory)
        {
            _hrDbContextFactory = hrDbContextFactory;
        }

        public override async Task<ImportUsersOutput> ExecuteAsync(ImportUsersInput input, ImportUsersOutput output)
        {
            await using var dbContext =
                await _hrDbContextFactory.CreateDbContextAsync();

            var importData = SeedDataGenerator
                .GenerateIdentityUserSeedData(input.ImportCountCount);

            await dbContext.AddBulkAsync(importData, input.ImportBatchSize);

            output.TotalRecordsProcessed = importData.Count;

            return output;
        }
    }
}