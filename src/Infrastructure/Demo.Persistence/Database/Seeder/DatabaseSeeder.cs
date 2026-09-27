using Demo.Persistence.DbContexts;
using Demo.Persistence.DbModels;

using Microsoft.EntityFrameworkCore;

namespace Demo.Persistence.Database.Seeder
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

            /*
             * Seed only the reference data required by the demo.
             *
             * Identity users are imported through the Auth workflow.
             * Employees and all downstream onboarding records are created
             * by the employee onboarding workflow.
             */

            var employerExists =
                await dbContext.Set<Employer>()
                    .AsNoTracking()
                    .AnyAsync(employer =>
                        employer.Name == "SolidArcSoftware");

            if (!employerExists)
            {
                var employer = new Employer
                {
                    Name = "SolidArcSoftware",
                    Description =
                        "Demo employer used by the DataArc employee onboarding workflow."
                };

                await dbContext.Set<Employer>()
                    .AddAsync(employer);
            }

            var departmentExists =
                await dbContext.Set<Department>()
                    .AsNoTracking()
                    .AnyAsync(department =>
                        department.Name == "Information Technology");

            if (!departmentExists)
            {
                var department = new Department
                {
                    Name = "Information Technology",
                    Description =
                        "Information Technology Department"
                };

                await dbContext.Set<Department>()
                    .AddAsync(department);
            }

            if (dbContext.ChangeTracker.HasChanges())
            {
                rowsAffected +=
                    await dbContext.SaveChangesAsync();
            }

            return rowsAffected;
        }
    }
}