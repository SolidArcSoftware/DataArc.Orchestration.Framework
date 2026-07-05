using DataArc.Orchestration.Framework.Demo.Application.Features.HR.EmployeeOnboarding.Dtos;
using DataArc.Orchestration.Framework.Demo.Application.Features.HR.Repository;
using DataArc.Orchestration.Framework.Demo.Persistence.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DataArc.Orchestration.Framework.Demo.Application.Modules.HR.Repository
{
    internal class HRRepository : IHRRepository
    {
        readonly IHrDbContext _hrDbContext;
        public HRRepository(IHrDbContext hrDbContext)
        {
            _hrDbContext = hrDbContext;
        }

        public async Task<IReadOnlyCollection<OnboardEmployeeRequestDto>> GetEmployeeOnboardingDetails(int MinimumRating)
        {
            var candidates = await _hrDbContext!.Employee!
                 .Where(emp => emp.Rating >= MinimumRating)
                 .Select(x => new OnboardEmployeeRequestDto()
                 {
                     AnnualSalary = x.Salary,
                     EmployeeId = x.EmployerId,
                 }).ToListAsync();

            return new List<OnboardEmployeeRequestDto>();
        }
    }
}