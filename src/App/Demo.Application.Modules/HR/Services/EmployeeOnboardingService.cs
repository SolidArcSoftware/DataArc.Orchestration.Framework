using DataArc.Observer;

using Demo.Application.Domain.HR.Policies;
using Demo.Application.Domain.HR.Policies.Contexts;
using Demo.Application.Domain.HR.ValueObjects;
using Demo.Application.Features.HR.EmployeeOnboarding.Dtos;
using Demo.Application.Features.HR.EmployeeOnboarding.Services;

using Demo.Orchestration.HR.Orchestrators.Input;
using Demo.Orchestration.HR.Ports;

namespace Demo.Application.Modules.Modules.HR.Services
{
    internal sealed class EmployeeOnboardingService : IEmployeeOnboardingService
    {
        private readonly IHROrchestrationPort _hrOrchestration;
        private readonly IEmployeeOnboardingPolicy _employeeOnboardingPolicy;
        private readonly IObservableEventHandler _observableEventHandler;

        public EmployeeOnboardingService(
            IHROrchestrationPort hrOrchestration,
            IObservableEventHandler observableEventHandler,
            IEmployeeOnboardingPolicy employeeOnboardingPolicy)
        {
            _hrOrchestration = hrOrchestration;
            _observableEventHandler = observableEventHandler;
            _employeeOnboardingPolicy = employeeOnboardingPolicy;
        }

        public async Task<OnboardEmployeeResponseDto> OnboardEmployeeAsync(OnboardEmployeeRequestDto request)
        {
            /*
             * Prepare the onboarding state across the participating
             * application persistence boundaries.
             */
            var preparedEmployee = await _hrOrchestration.PrepareEmployeeOnboardingAsync(new PrepareEmployeeOnboardingInput{UserId = request.UserId});

            if (!preparedEmployee.IsSuccess)
            {
                return new OnboardEmployeeResponseDto
                {
                    IsSuccess = false,
                    FailureReason = preparedEmployee.FailureReason
                };
            }

            /*
             * The onboarding candidate must originate from Identity.
             */
            if (!preparedEmployee.UserExists)
            {
                return new OnboardEmployeeResponseDto
                {
                    IsSuccess = false,
                    FailureReason =
                        "The selected identity user could not be found."
                };
            }

            /*
             * Build the business-policy context from the persisted
             * state prepared by the orchestration layer.
             *
             * No persistence mechanics leak into the domain policy.
             */
            var policyContext = new OnboardEmployeePolicyContext(
                new OnBoardEmployeeValueObject(
                    preparedEmployee.OnBoardingStatus),
                    preparedEmployee.EmployeeExists,
                    preparedEmployee.HasDepartment,
                    preparedEmployee.PayrollRecordExists,
                    preparedEmployee.AccessRequestExists,
                    preparedEmployee.OnboardingTaskExists);

            var policyResult = _employeeOnboardingPolicy.Apply(policyContext);

            /*
             * Publish the domain events produced by the policy decision.
             */
            await _observableEventHandler.DispatchAsync(policyResult.DomainEvents);

            if (!policyResult.IsSuccess)
            {
                return new OnboardEmployeeResponseDto
                {
                    IsSuccess = false,
                    FailureReason = policyResult.Message,
                };
            }

            /*
             * The policy has approved the workflow.
             * The orchestration layer now performs the coordinated
             * persistence operation.
             */
            var output = 
                await _hrOrchestration.OnboardEmployeeAsync(
                    new OnboardEmployeeInput(
                        request.UserId,
                        request.AnnualSalary,
                        request.CurrencyCode,
                        request.Reason,
                        DateTimeOffset.UtcNow));

            if (!output.IsSuccess)
            {
                return new OnboardEmployeeResponseDto
                {
                    IsSuccess = false,
                    FailureReason = output.FailureReason
                };
            }

            return new OnboardEmployeeResponseDto
            {
                IsSuccess = true,
                FailureReason = null,
                EmployeeId = output.EmployeeId,
                PayrollRecordId = output.PayrollRecordId
            };
        }
    }
}