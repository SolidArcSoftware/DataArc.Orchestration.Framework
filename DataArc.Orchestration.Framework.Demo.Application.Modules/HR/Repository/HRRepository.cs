using DataArc.Core;
using DataArc.Orchestration.Framework.Demo.Application.Features.HR.EmployeeOnboarding.Dtos;
using DataArc.Orchestration.Framework.Demo.Application.Features.HR.Repository;
using DataArc.Orchestration.Framework.Demo.Persistence.Contracts;
using DataArc.Orchestration.Framework.Demo.Persistence.DbModels;

namespace DataArc.Orchestration.Framework.Demo.Application.Modules.HR.Repository
{
    internal class HRRepository : IHRRepository
    {
        private readonly IQueryFactory _queryFactory;

        public HRRepository(
            IQueryFactory queryFactory)
        {
            _queryFactory = queryFactory;
        }
        public async Task<OnboardEmployeeCandidateDto?> GetEmployeeOnboardingCandidate(int employeeId)
        {
            var hrQuery = await _queryFactory.CreateQueryAsync();
            var candidates = await hrQuery
                .UseDbExecutionContext<IHrDbContext>()
                .ReadWhereAsync<Employee>(emp => emp.Id == employeeId);

            var candidate = candidates
                .Select(emp => new OnboardEmployeeCandidateDto
                {
                    IsSuccess = true,
                    FailureReason = null,
                    EmployeeId = emp.Id,
                    EmployeeName = emp.Name,
                    EmployeeNameSurname = emp.Surname,
                    Status = emp.OnBoardingStatus,
                    Rating = emp.Rating,
                    EmployeeSalary = emp.Salary
                })
                .FirstOrDefault();

            return candidate;
        }
    }
}