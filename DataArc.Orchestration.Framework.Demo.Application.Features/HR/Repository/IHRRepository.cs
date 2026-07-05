using DataArc.Orchestration.Framework.Demo.Application.Features.HR.EmployeeOnboarding.Dtos;

namespace DataArc.Orchestration.Framework.Demo.Application.Features.HR.Repository
{
    public interface IHRRepository
    {
        Task<IReadOnlyCollection<OnboardEmployeeRequestDto>> GetEmployeeOnboardingDetails(int MinimumRating);
    }
}