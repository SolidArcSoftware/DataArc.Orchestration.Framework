using Demo.Application.Domain.Entities;
using Demo.Application.Features.HR.Repositories;
using Demo.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Demo.Application.Modules.Modules.HR.Repositories
{
    internal class HRRepository : IHRRepository
    {
        private readonly IDbContextFactory<HrDbContext> _dbContextFactory;

        public HRRepository(
            IDbContextFactory<HrDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<EmployeeEntity?> GetEmployeeOnboardingCandidate(int employeeId)
        {
            try
            {
                await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

                return await dbContext.Employee!
                    .AsNoTracking()
                    .Where(emp => emp.Id == employeeId)
                    .Select(emp => new EmployeeEntity
                    {
                        Id = emp.Id,
                        Name = emp.Name,
                        Surname = emp.Surname,
                        Salary = emp.Salary,
                        EmployerId = emp.EmployerId,
                        Order = emp.Order,
                        IsArchived = emp.IsArchived,
                        CreatedUtc = emp.CreatedUtc,
                        LastUpdatedUtc = emp.LastUpdatedUtc,
                        Notes = emp.Notes,
                        OnBoardingStatus = emp.OnBoardingStatus,
                        Rating = emp.Rating
                    })
                    .FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}