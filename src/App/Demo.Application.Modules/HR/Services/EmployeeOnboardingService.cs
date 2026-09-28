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

        public async Task<OnboardEmployeeResponseDto> OnboardEmployeeAsync(
            OnboardEmployeeRequestDto request)
        {
            /*
             * Gather the persisted state required to evaluate the
             * employee onboarding policy.
             *
             * The preparation orchestration discovers facts across the
             * participating persistence boundaries. It does not decide
             * whether the onboarding request is allowed.
             */
            var preparedEmployee =
                await _hrOrchestration.PrepareEmployeeOnboardingAsync(
                    new PrepareEmployeeOnboardingInput
                    {
                        UserName = request.UserName
                    });

            /*
             * Translate the prepared persistence state into the rich
             * domain value used by the onboarding policy.
             *
             * The policy owns the business decision. The service only
             * coordinates the prepared state, policy evaluation and
             * subsequent orchestration.
             */
            var policyContext =
                new OnboardEmployeePolicyContext(
                    new OnBoardEmployeeValueObject(
                        preparedEmployee.OnBoardingStatus,
                        preparedEmployee.UserExists,
                        preparedEmployee.EmployeeExists,
                        preparedEmployee.HasDepartment,
                        preparedEmployee.PayrollRecordExists,
                        preparedEmployee.AccessRequestExists,
                        preparedEmployee.OnboardingTaskExists));

            var policyResult = _employeeOnboardingPolicy.Apply(policyContext);

            /*
             * Publish the domain events produced by the policy decision.
             *
             * Observers record the decision through logs, traces and metrics,
             * while the PolicyResult remains the source of the response
             * returned to the caller.
             */
            await _observableEventHandler.DispatchAsync(policyResult.DomainEvents);

            if (!policyResult.IsSuccess)
            {
                return new OnboardEmployeeResponseDto
                {
                    IsSuccess = false,
                    FailureReason = policyResult.Message
                };
            }

            /*
             * The domain policy has approved the onboarding use case.
             *
             * Assemble the execution instruction from the resolved Identity
             * data, request data and approved workflow values, then hand it
             * to the orchestration layer for coordinated persistence.
             *
             * The execution orchestrator performs no business-rule decisions;
             * it persists the supplied instruction across the participating
             * application boundaries.
             */
            var effectiveOnUtc = DateTimeOffset.UtcNow;

            var output =
                await _hrOrchestration.OnboardEmployeeAsync(
                    new OnboardEmployeeInput(
                        UserId: preparedEmployee.UserId,
                        EmailAddress: preparedEmployee.EmailAddress!,
                        AnnualSalary: request.AnnualSalary,
                        CurrencyCode: request.CurrencyCode,
                        Reason: request.Reason,
                        EmployeeOnboardingStatus: "Completed",
                        AccessLevel: "Standard",
                        AccessRequestStatus: "Requested",
                        OnboardingTaskName: "Complete employee onboarding",
                        OnboardingTaskStatus: "Created",
                        PayrollIsActive: true,
                        EffectiveOnUtc: effectiveOnUtc,
                        DueDateOnUtc: effectiveOnUtc.AddMonths(1)));
            /*
             * A normally returned orchestration output means the coordinated
             * persistence operation completed successfully. Technical or
             * persistence failures propagate as exceptions instead.
             */
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