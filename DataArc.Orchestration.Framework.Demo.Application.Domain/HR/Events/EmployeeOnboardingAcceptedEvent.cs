using DataArc.Orchestration.Framework.Demo.Application.Domain.SharedKernel;

namespace DataArc.Orchestration.Framework.Demo.Application.Domain.HR.Events
{
    public record EmployeeOnboardingAcceptedEvent(
        int EmployeeId,
        decimal AnnualSalary,
        string CurrencyCode,
        string Reason,
        DateTimeOffset EffectiveOnUtc
    ) : IDomainEvent;
}