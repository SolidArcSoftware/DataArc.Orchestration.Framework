using DataArc.Orchestration.Framework.Demo.Application.Domain.Modules.Finance.ValueObjects;

namespace DataArc.Orchestration.Framework.Demo.Application.Domain.Modules.Finance.Policies.Context
{
    public sealed class SalaryAdjustmentPolicyContext
    {
        public SalaryAdjustmentValueObject SalaryAdjustmentValueObject { get; }

        public SalaryAdjustmentPolicyContext(SalaryAdjustmentValueObject salaryAdjustmentValueObject)
        {
            SalaryAdjustmentValueObject = salaryAdjustmentValueObject;
        }
    }
}