using DataArc.Orchestration.Framework.Demo.Application.Modules.Finance.UseCases;

namespace DataArc.Orchestration.Framework.Demo.Application.UseCases.Modules.Finance.Ports
{
    public interface IFinanceOrchestrationPort
    {
        Task<PrepareTopRatedEmployeesOutput> PrepareTopRatedEmployeesAsync(
            PrepareTopRatedEmployeesInput input,
            CancellationToken cancellationToken = default);

        Task<ProcessEmployeeSalaryAdjustmentsOutput> ProcessEmployeeSalaryAdjustmentAsync(
            ProcessEmployeeSalaryAdjustmentsInput input,
            CancellationToken cancellationToken = default);
    }
}