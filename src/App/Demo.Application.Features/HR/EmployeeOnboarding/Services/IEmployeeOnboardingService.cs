using Demo.Application.Features.HR.EmployeeOnboarding.Dtos;

namespace Demo.Application.Features.HR.EmployeeOnboarding.Services
{
    public interface IEmployeeOnboardingService
    {
        Task<OnboardEmployeeResponseDto> OnboardEmployeeAsync(OnboardEmployeeRequestDto onboardEmployeeRequest);
    }
}