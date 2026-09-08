using DataArc.Orchestration.Framework.Demo.Application.Domain.HR.Entities;

namespace DataArc.Orchestration.Framework.Demo.Application.Features.HR.Repositories
{
    public interface IHRRepository
    {
        public Task<EmployeeEntity?> GetEmployeeOnboardingCandidate(int employeeId);
    }
}