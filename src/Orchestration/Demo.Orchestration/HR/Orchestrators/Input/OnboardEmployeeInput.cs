using DataArc.Orchestrator;

namespace Demo.Orchestration.HR.Orchestrators.Input
{
    public sealed record OnboardEmployeeInput(
       int UserId,
       string EmailAddress,
       decimal AnnualSalary,
       string CurrencyCode,
       string Reason,
       string EmployeeOnboardingStatus,
       string AccessLevel,
       string AccessRequestStatus,
       string OnboardingTaskName,
       string OnboardingTaskStatus,
       bool PayrollIsActive,
       DateTimeOffset EffectiveOnUtc,
       DateTimeOffset DueDateOnUtc
        ) : IOrchestratorInput;
}