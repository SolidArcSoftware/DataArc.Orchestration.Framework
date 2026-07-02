using DataArc.Demo.Domain.SharedKernel;

namespace DataArc.Orchestration.Framework.Demo.Application.Domain.Modules.Finance.Events
{
    public sealed record RejectEmployeeSalaryAdjustmentEvent(
       int EmployeeId,
       string? Name,
       string? Surname,
       double? Rating,
       string Reason) : IDomainEvent;
}