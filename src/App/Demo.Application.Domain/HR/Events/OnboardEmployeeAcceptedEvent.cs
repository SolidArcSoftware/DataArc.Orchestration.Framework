using Demo.Application.Domain.SharedKernel;

namespace Demo.Application.Domain.HR.Events
{
    public record class OnboardEmployeeAcceptedEvent(string? Reason) : IDomainEvent;
}