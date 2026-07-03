using DataArc.Orchestration.Framework.Demo.Application.Modules.Finance.UseCases;
using DataArc.Orchestration.Framework.Demo.Persistence.Contracts;
using DataArc.Orchestration.Framework.Demo.Persistence.DbModels;

using DataArc.Core;
using DataArc.Orchestrator;

namespace DataArc.Orchestration.Framework.Demo.Orchestration.Orchestrators
{
    public class ProcessEmployeeSalaryAdjustmentsOrchestrator 
        : Orchestrator<ProcessEmployeeSalaryAdjustmentsInput, ProcessEmployeeSalaryAdjustmentsOutput>
    {
        readonly ICommandFactory _commandFactory;
        readonly IQueryFactory _queryFactory;
        public ProcessEmployeeSalaryAdjustmentsOrchestrator(
            ICommandFactory commandFactory, IQueryFactory queryFactory)
        {
            _commandFactory = commandFactory;
            _queryFactory = queryFactory;
        }

        public override async Task<ProcessEmployeeSalaryAdjustmentsOutput> ExecuteAsync(
            ProcessEmployeeSalaryAdjustmentsInput input, ProcessEmployeeSalaryAdjustmentsOutput output)
        {
            var employeesQuery = await _queryFactory.CreateQueryAsync();
            var employees = await employeesQuery
                .UseDbExecutionContext<IHrDbContext>()
                    .ReadWhereAsync<Employee>(e => e.Salary > input.salaryThreshold);

            foreach (var employee in employees)
            {
                employee.Salary += employee.Salary * input.salaryAdjustmentBaseRate;
            }

            var commandBuilder = await _commandFactory
                .CreateCommandBuilderAsync();

            commandBuilder
                .UseDbExecutionContext<IFinanceDbContext>()
                .AddBulk(employees, input.batchSize);

            commandBuilder
                .UseDbExecutionContext<IItDbContext>()
                .AddBulk(employees, input.batchSize);

            commandBuilder
                .UseDbExecutionContext<IOperationsDbContext>()
                .AddBulk(employees, input.batchSize);

            var command = await commandBuilder.BuildAsync();
            var commandResult = await command.ExecuteAsync();

            if (!commandResult.Success)
            {
                throw new InvalidOperationException($"Failed to process salary adjustments. {commandResult?.Exception?.Message}");
            }

            output.TotalRecordsProcessed = commandResult.TotalAffected;
            return output;
        }
    }
}