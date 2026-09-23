using Demo.Application.Domain.SharedKernel;

namespace Demo.Application.Domain.HR.Events
{
    public record OnboardEmployeeRejectedEvent(string? Reason) : IDomainEvent;
}