using DataArc.Orchestration.Framework.Demo.Application.UseCases.HR.Orchestration.Input;
using DataArc.Orchestration.Framework.Demo.Application.UseCases.HR.Orchestration.Ouput;
using DataArc.Orchestration.Framework.Demo.Persistence.Contracts;
using DataArc.Orchestration.Framework.Demo.Persistence.Utils;

using DataArc.Core;
using DataArc.Orchestrator;

namespace DataArc.Orchestration.Framework.Demo.Orchestration.HR.Orchestrators
{
    public class ImportEmployeesDataOrchestrator : Orchestrator<ImportEmployeesInput, ImportEmployeesOutput>
    {
        readonly ICommandFactory _commandFactory;

        public ImportEmployeesDataOrchestrator(ICommandFactory commandFactory)
        {
            _commandFactory = commandFactory;
        }

        public override async Task<ImportEmployeesOutput> ExecuteAsync(ImportEmployeesInput input, ImportEmployeesOutput output)
        {
            try
            {
                var importTransactionalCommand = await _commandFactory
                    .CreateTransactionalCommandAsync();

                var importData = SeedDataGenerator
                    .GenerateHrSeedData(input.ImportEmployeeCount);

                var importTransaction = importTransactionalCommand
                    .UseDbExecutionContext<IHrDbContext>()
                    .AddBulk(importData, input.ImportBatchSize);

                var transactionResult 
                    = await importTransaction.CommitTransactionAsync();

                if (!transactionResult.Success)
                {
                    output.Errors = transactionResult?.Errors;
                }

                output.TotalRecordsProcessed = transactionResult!.TotalAffected;
                return output;
            }
            catch
            {
                throw;
            }
        }
    }
}