using DataArc.Observer;
using DataArc.Orchestration.Framework.Demo.Application.Domain.HR.Policies;
using DataArc.Orchestration.Framework.Demo.Application.Domain.HR.Policies.Contexts;
using DataArc.Orchestration.Framework.Demo.Application.Domain.HR.ValueObjects;
using DataArc.Orchestration.Framework.Demo.Application.Features.HR.EmployeeOnboarding.Dtos;
using DataArc.Orchestration.Framework.Demo.Application.UseCases.HR.Orchestration.Input;
using DataArc.Orchestration.Framework.Demo.Application.UseCases.HR.Ports;

namespace DataArc.Orchestration.Framework.Demo.Application.Features.HR.EmployeeOnboarding.Services
{
    public sealed class EmployeeOnboardingService : IEmployeeOnboardingService
    {
        private readonly IHROrchestrationPort _HROrchestrationPort;
        private readonly IEmployeeOnboardingPolicy _onboardEmployeePolicy;
        private readonly IObservableEventHandler _observableEventHandler;

        public EmployeeOnboardingService(
            IHROrchestrationPort HROrchestrationPort,
            IObservableEventHandler observableEventHandler,
            IEmployeeOnboardingPolicy onboardEmployeePolicy)
        {
            _HROrchestrationPort = HROrchestrationPort;
            _onboardEmployeePolicy = onboardEmployeePolicy;
            _observableEventHandler = observableEventHandler;
        }

        public async Task<OnboardEmployeeResponseDto> OnboardEmployeeAsync(
            OnboardEmployeeRequestDto request)
        {
            var employee = await _HROrchestrationPort.PrepareEmployeeOnboardingAsync(
                new PrepareEmployeeOnboardingInput(request.EmployeeId));

            if (employee == null || !employee.IsSuccess)
            {
                var failureReason = employee?.FailureReason
                    ?? "Employee onboarding context could not be prepared.";

                return new OnboardEmployeeResponseDto
                {
                    IsSuccess = false,
                    EmployeeId = request.EmployeeId,
                    FailureReason = failureReason
                };
            }

            var onboardingCandidate = new OnBoardEmployeeValueObject(
                status: employee.Status,
                payrollRecordId: employee.PayrollRecordId,
                accessRequestId: employee.AccessRequestId,
                onboardingTaskId: employee.OnboardingTaskId,
                payrollCurrencyCode: employee.PayrollCurrencyCode,
                payrollIsActive: employee.PayrollIsActive,
                accessRequestStatus: employee.AccessRequestStatus,
                onboardingTaskStatus: employee.OnboardingTaskStatus);

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

            var output = await _HROrchestrationPort.OnboardEmployeeAsync(
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