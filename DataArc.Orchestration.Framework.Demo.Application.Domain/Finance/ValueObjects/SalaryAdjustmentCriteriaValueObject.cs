namespace DataArc.Orchestration.Framework.Demo.Application.Domain.Modules.Finance.ValueObjects
{
    public sealed class SalaryAdjustmentCriteriaValueObject
    {
        public decimal SalaryAdjustmentBaseRate { get; }
        public decimal SalaryThreshold { get; }
        public int BatchSize { get; }

        public SalaryAdjustmentCriteriaValueObject(
            decimal salaryAdjustmentBaseRate,
            decimal salaryThreshold,
            int batchSize)
        {
            SalaryAdjustmentBaseRate = salaryAdjustmentBaseRate;
            SalaryThreshold = salaryThreshold;
            BatchSize = batchSize;
        }

        public bool IsValid =>
            SalaryAdjustmentBaseRate > 0 &&
            SalaryThreshold > 0 &&
            BatchSize > 0;
    }
}