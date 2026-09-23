using Demo.Application.Domain.Entities;

namespace Demo.Application.Features.HR.Repositories
{
    public interface IHRRepository
    {
        public Task<EmployeeEntity?> GetEmployeeOnboardingCandidate(int employeeId);
    }
}