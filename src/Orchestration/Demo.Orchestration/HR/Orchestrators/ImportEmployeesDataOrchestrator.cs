using DataArc.EntityFrameworkCore;
using Demo.Orchestration.HR.Orchestrators.Input;
using Demo.Orchestration.HR.Orchestrators.Ouput;
using Demo.Persistence.DbContexts;
using Demo.Persistence.Utils;
using DataArc.Orchestrator;
using Microsoft.EntityFrameworkCore;

namespace Demo.Orchestration.HR.Orchestrators
{
    public sealed class ImportEmployeesDataOrchestrator
        : Orchestrator<ImportEmployeesInput, ImportEmployeesOutput>
    {
        private readonly IDbContextFactory<HrDbContext> _hrDbContextFactory;

        public ImportEmployeesDataOrchestrator(
            IDbContextFactory<HrDbContext> hrDbContextFactory)
        {
            _hrDbContextFactory = hrDbContextFactory;
        }

        public override async Task<ImportEmployeesOutput> ExecuteAsync(
            ImportEmployeesInput input,
            ImportEmployeesOutput output)
        {
            await using var dbContext =
                await _hrDbContextFactory.CreateDbContextAsync();

            var importData = SeedDataGenerator
                .GenerateHrSeedData(input.ImportEmployeeCount);

            await dbContext.AsParallel()
                .AddBulk(importData, input.ImportBatchSize)
                .SaveChangesParallelAsync();

            output.TotalRecordsProcessed = importData.Count;

            return output;
        }
    }
}