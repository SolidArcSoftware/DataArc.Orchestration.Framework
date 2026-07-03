using DataArc.Orchestration.Framework.Demo.Application.Domain.SharedKernel;

namespace DataArc.Orchestration.Framework.Demo.Application.Domain.Modules.Finance.Events
{
    public sealed record EmployeeSalaryAdjustmentRejectedEvent(
       int EmployeeId,
       string? Name,
       string? Surname,
       double? Rating,
       string Reason) : IDomainEvent;
}