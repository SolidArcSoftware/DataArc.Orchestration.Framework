using DataArc.Orchestration.Framework.Demo.Application.Domain.SharedKernel;

namespace DataArc.Orchestration.Framework.Demo.Application.Domain.HR.Events
{
    public record OnboardEmployeeRejectedEvent(string? Reason) : IDomainEvent;
}