using DataArc.Orchestration.Framework.Demo.Application.Domain.SharedKernel;

namespace DataArc.Orchestration.Framework.Demo.Application.Domain.HR.Events
{
    public record class OnboardEmployeeAcceptedEvent(string? Reason) : IDomainEvent;
}