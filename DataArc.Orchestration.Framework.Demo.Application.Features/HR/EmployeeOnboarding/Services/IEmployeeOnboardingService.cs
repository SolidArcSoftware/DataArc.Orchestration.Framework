using DataArc.Orchestration.Framework.Demo.Application.Features.HR.EmployeeOnboarding.Dtos;

namespace DataArc.Orchestration.Framework.Demo.Application.Features.HR.EmployeeOnboarding.Services
{
    public interface IEmployeeOnboardingService
    {
        Task<OnboardEmployeeResponseDto> OnboardEmployeeAsync(OnboardEmployeeRequestDto onboardEmployeeRequest);
    }
}