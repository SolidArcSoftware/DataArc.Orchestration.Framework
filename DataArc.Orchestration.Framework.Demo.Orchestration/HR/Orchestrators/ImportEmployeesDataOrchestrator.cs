using DataArc.EntityFrameworkCore;
using DataArc.Orchestration.Framework.Demo.Orchestration.HR.Orchestrators.Input;
using DataArc.Orchestration.Framework.Demo.Orchestration.HR.Orchestrators.Ouput;
using DataArc.Orchestration.Framework.Demo.Persistence.DbContexts;
using DataArc.Orchestration.Framework.Demo.Persistence.Utils;
using DataArc.Orchestrator;
using Microsoft.EntityFrameworkCore;

namespace DataArc.Orchestration.Framework.Demo.Orchestration.HR.Orchestrators
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

            await dbContext.AddBulkAsync(
                importData,
                input.ImportBatchSize);

            output.TotalRecordsProcessed = importData.Count;

            return output;
        }
    }
}