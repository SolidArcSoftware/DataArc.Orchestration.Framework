using DataArc.Orchestrator;

namespace DataArc.Orchestration.Framework.Demo.Application.Modules.Finance.UseCases
{
    public record ProcessEmployeeSalaryAdjustmentsInput(
            decimal salaryAdjustmentBaseRate,
            decimal salaryThreshold,
            int batchSize) : IOrchestratorInput;
}