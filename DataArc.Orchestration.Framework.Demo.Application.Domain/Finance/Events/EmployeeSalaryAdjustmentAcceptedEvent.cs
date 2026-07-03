using DataArc.Orchestration.Framework.Demo.Application.Domain.SharedKernel;

namespace DataArc.Orchestration.Framework.Demo.Application.Domain.Modules.Finance.Events
{
    public sealed record EmployeeSalaryAdjustmentAcceptedEvent(
            decimal salaryAdjustmentBaseRate,
            decimal salaryThreshold,
            int batchSize) : IDomainEvent;
}