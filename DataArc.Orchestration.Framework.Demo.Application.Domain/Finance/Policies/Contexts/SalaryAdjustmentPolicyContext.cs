using DataArc.Orchestration.Framework.Demo.Application.Domain.Modules.Finance.ValueObjects;

namespace DataArc.Orchestration.Framework.Demo.Application.Domain.Modules.Finance.Policies.Context
{
    public sealed class SalaryAdjustmentPolicyContext
    {
        public SalaryAdjustmentValueObject SalaryAdjustmentValueObject { get; }
        public SalaryAdjustmentCriteriaValueObject SalaryAdjustmentCriteriaValueObject { get; }

        public SalaryAdjustmentPolicyContext(
            SalaryAdjustmentValueObject salaryAdjustmentValueObject, 
            SalaryAdjustmentCriteriaValueObject salaryAdjustmentCriteriaValueObject)
        {
            SalaryAdjustmentValueObject = salaryAdjustmentValueObject;
            SalaryAdjustmentCriteriaValueObject = salaryAdjustmentCriteriaValueObject;
        }
    }
}