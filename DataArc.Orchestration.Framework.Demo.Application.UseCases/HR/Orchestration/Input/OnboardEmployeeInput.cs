using DataArc.Orchestrator;

namespace DataArc.Orchestration.Framework.Demo.Application.UseCases.HR.Orchestration.Input
{
    public sealed record OnboardEmployeeInput(
      int EmployeeId,
      decimal AnnualSalary,
      string CurrencyCode,
      string Reason,
      DateTimeOffset EffectiveOnUtc) : IOrchestratorInput;
}