using DataArc.Orchestrator;

namespace Demo.Orchestration.HR.Orchestrators.Input
{
    public sealed record OnboardEmployeeInput(
        int UserId,
        string EmailAddress,
        decimal AnnualSalary,
        string CurrencyCode,
        string Reason,
        DateTimeOffset EffectiveOnUtc) : IOrchestratorInput;
}