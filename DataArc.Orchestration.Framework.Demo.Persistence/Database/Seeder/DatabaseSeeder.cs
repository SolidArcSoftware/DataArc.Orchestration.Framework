using DataArc.Orchestration.Framework.Demo.Persistence.DbContexts;
using DataArc.Orchestration.Framework.Demo.Persistence.DbModels;
using Microsoft.EntityFrameworkCore;

namespace DataArc.Orchestration.Framework.Demo.Persistence.Database.Seeder
{
    internal sealed class DatabaseSeeder : IDatabaseSeeder
    {
        private readonly IDbContextFactory<HrDbContext> _hrDbContextFactory;

        public DatabaseSeeder(
            IDbContextFactory<HrDbContext> hrDbContextFactory)
        {
            _hrDbContextFactory = hrDbContextFactory;
        }

        public async Task<int> SeedDatabaseAsync()
        {
            int rowsAffected = 0;

            await using var dbContext =
                await _hrDbContextFactory.CreateDbContextAsync();

            var createdUtc = DateTime.UtcNow;

            var employer = new Employer
            {
                Name = "Solid Arc Software",
                Description =
                    "Demo employer used by the DataArc Orchestration Framework integration tests."
            };

            dbContext.Set<Employer>().Add(employer);

            rowsAffected += await dbContext.SaveChangesAsync();

            var employee = new Employee
            {
                Name = "Demo",
                Surname = "Employee",
                Salary = 95000.00m,
                EmployerId = employer.Id,
                Order = 1,
                IsArchived = false,
                CreatedUtc = createdUtc,
                LastUpdatedUtc = null,
                Notes =
                    "Seed employee used for the onboarding orchestration workflow.",
                OnBoardingStatus = "Pending",
                Rating = 4.8
            };

            dbContext.Set<Employee>().Add(employee);

            rowsAffected += await dbContext.SaveChangesAsync();

            return rowsAffected;
        }
    }
}