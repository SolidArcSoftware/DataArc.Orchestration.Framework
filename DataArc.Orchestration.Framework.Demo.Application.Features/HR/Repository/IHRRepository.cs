using DataArc.Orchestration.Framework.Demo.Application.Features.HR.EmployeeOnboarding.Dtos;

namespace DataArc.Orchestration.Framework.Demo.Application.Features.HR.Repository
{
    public interface IHRRepository
    {
        public Task<OnboardEmployeeCandidateDto?> GetEmployeeOnboardingCandidate(int employeeId);
    }
}