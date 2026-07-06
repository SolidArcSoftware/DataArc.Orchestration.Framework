using DataArc.Observer;
using DataArc.Orchestration.Framework.Demo.Application.Domain.HR.Policies;
using DataArc.Orchestration.Framework.Demo.Application.Domain.HR.Policies.Contexts;
using DataArc.Orchestration.Framework.Demo.Application.Domain.HR.ValueObjects;
using DataArc.Orchestration.Framework.Demo.Application.Features.HR.EmployeeOnboarding.Dtos;
using DataArc.Orchestration.Framework.Demo.Application.Features.HR.Repository;
using DataArc.Orchestration.Framework.Demo.Application.UseCases.HR.Orchestration.Input;
using DataArc.Orchestration.Framework.Demo.Application.UseCases.HR.Ports;

namespace DataArc.Orchestration.Framework.Demo.Application.Features.HR.EmployeeOnboarding.Services
{
    internal sealed class EmployeeOnboardingService : IEmployeeOnboardingService
    {
        private readonly IHRRepository _hRRepository;
        private readonly IHROrchestration _HROrchestration;
        private readonly IEmployeeOnboardingPolicy _onboardEmployeePolicy;
        private readonly IObservableEventHandler _observableEventHandler;

        public EmployeeOnboardingService(
            IHRRepository hRRepository,
            IHROrchestration HROrchestration,
            IObservableEventHandler observableEventHandler,
            IEmployeeOnboardingPolicy onboardEmployeePolicy)
        {
            _hRRepository = hRRepository;
            _HROrchestration = HROrchestration;
            _onboardEmployeePolicy = onboardEmployeePolicy;
            _observableEventHandler = observableEventHandler;
        }

        public async Task<OnboardEmployeeResponseDto> OnboardEmployeeAsync(
            OnboardEmployeeRequestDto request)
        {
            var employee = await _hRRepository.GetEmployeeOnboardingCandidate(request.EmployeeId);

            if (employee == null)
            {
                return new OnboardEmployeeResponseDto
                {
                    IsSuccess = false,
                    EmployeeId = request.EmployeeId,
                    FailureReason = "Employee onboarding context could not be prepared."
                };
            }

            var onboardingCandidate = new OnBoardEmployeeValueObject(status: employee.Status);

            var policyContext = new OnboardEmployeePolicyContext(
                onboardingCandidate);

            var policyResult = _onboardEmployeePolicy.Apply(
                policyContext);

            await _observableEventHandler.DispatchAsync(
                policyResult.DomainEvents);

            if (!policyResult.IsSuccess)
            {
                return new OnboardEmployeeResponseDto
                {
                    IsSuccess = false,
                    EmployeeId = employee.EmployeeId,
                    Name = employee.EmployeeName,
                    Surname = employee.EmployeeNameSurname,
                    Status = employee.Status,
                    Rating = employee.Rating,
                    Salary = employee.EmployeeSalary,
                    FailureReason = policyResult.Message
                };
            }

            var output = await _HROrchestration.OnboardEmployeeAsync(
                new OnboardEmployeeInput(
                    request.EmployeeId,
                    request.AnnualSalary,
                    request.CurrencyCode,
                    request.Reason,
                    DateTimeOffset.UtcNow));

            return new OnboardEmployeeResponseDto
            {
                IsSuccess = true,
                FailureReason = null,
                EmployeeId = employee.EmployeeId,
                Name = employee.EmployeeName,
                Surname = employee.EmployeeNameSurname,
                Status = employee.Status,
                Rating = employee.Rating,
                Salary = request.AnnualSalary,
                PayrollRecordId = output.PayrollRecordId,
                EmployeePayrollRecordId = output.EmployeePayrollRecordId
            };
        }
    }
}