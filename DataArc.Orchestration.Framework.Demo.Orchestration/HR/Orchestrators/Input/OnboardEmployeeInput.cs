using DataArc.Orchestrator;

namespace DataArc.Orchestration.Framework.Demo.Orchestration.HR.Orchestrators.Input
{
    public sealed record OnboardEmployeeInput(
      int EmployeeId,
      decimal AnnualSalary,
      string CurrencyCode,
      string Reason,
      DateTimeOffset EffectiveOnUtc) : IOrchestratorInput;
}