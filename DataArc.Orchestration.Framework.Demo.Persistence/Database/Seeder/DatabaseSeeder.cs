using DataArc.Core;
using DataArc.Orchestration.Framework.Demo.Persistence.Contracts;
using DataArc.Orchestration.Framework.Demo.Persistence.DbModels;

namespace DataArc.Orchestration.Framework.Demo.Persistence.Database.Seeder
{
    internal class DatabaseSeeder : IDatabaseSeeder
    {
        readonly ICommandFactory _commandFactory;
        public DatabaseSeeder(ICommandFactory commandFactory)
        {
            _commandFactory = commandFactory;
        }

        public async Task<CommandResult> SeedDatabaseAsync()
        {
            var createdUtc = DateTime.UtcNow;

            var employer = new Employer
            {
                Name = "Solid Arc Software",
                Description = "Demo employer used by the DataArc Orchestration Framework integration tests."
            };

            var employee = new Employee
            {
                Name = "Demo",
                Surname = "Employee",
                Salary = 95000.00m,
                EmployerId = 1,
                Order = 1,
                IsArchived = false,
                CreatedUtc = createdUtc,
                LastUpdatedUtc = null,
                Notes = "Seed employee used for the onboarding orchestration workflow.",
                OnBoardingStatus = "Pending",
                Rating = 4.8
            };

            var commandBuilder = await _commandFactory.CreateCommandBuilderAsync();

            commandBuilder
                .UseDbExecutionContext<IHrDbContext>()
                    .Add(employer)
                    .Add(employee);

            var command = await commandBuilder.BuildAsync();
            var commandResult = await command.ExecuteAsync();
           
            return commandResult;
        }
    }
}