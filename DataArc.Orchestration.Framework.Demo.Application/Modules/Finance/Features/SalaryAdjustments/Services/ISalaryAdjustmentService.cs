namespace DataArc.Orchestration.Framework.Demo.Modules.Finance.Features.SalaryAdjustments.Services
{
    public interface ISalaryAdjustmentService
    {
        Task<int> ProcessEmployeeSalaryAdjustmentsAsync(decimal salaryAdjustmentBaseRate, decimal salaryThreshold, int batchSize);
    }
}