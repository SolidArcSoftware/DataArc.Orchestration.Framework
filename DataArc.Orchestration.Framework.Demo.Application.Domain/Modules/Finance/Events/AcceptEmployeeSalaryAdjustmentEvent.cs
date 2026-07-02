using DataArc.Demo.Domain.SharedKernel;

namespace DataArc.Orchestration.Framework.Demo.Application.Domain.Modules.Finance.Events
{
    public sealed record AcceptEmployeeSalaryAdjustmentEvent(
        int EmployeeId,
        string? Name,
        string? Surname,
        decimal CurrentSalary,
        decimal AdjustedSalary,
        double? Rating) : IDomainEvent;
}