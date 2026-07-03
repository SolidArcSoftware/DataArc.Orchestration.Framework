using DataArc.Orchestration.Framework.Demo.Application.Domain.Modules.Finance.Events;
using DataArc.Orchestration.Framework.Demo.Application.Domain.Modules.Finance.Policies.Context;
using DataArc.Orchestration.Framework.Demo.Application.Domain.SharedKernel;

namespace DataArc.Orchestration.Framework.Demo.Application.Domain.Modules.Finance.Policies
{
    public sealed class SalaryAdjustmentPolicy : ISalaryAdjustmentPolicy
    {
        public PolicyResult Apply(SalaryAdjustmentPolicyContext context)
        {
            var salaryAdjustmentValueObject = context.SalaryAdjustmentValueObject;

            if (!salaryAdjustmentValueObject.IsQualifiedForAdjustment)
            {
                return PolicyResult.Fail(
                    "The employee is not qualified for a salary adjustment",
                    new EmployeeSalaryAdjustmentRejectedEvent(
                        salaryAdjustmentValueObject.EmployeeId,
                        salaryAdjustmentValueObject.Name,
                        salaryAdjustmentValueObject.Surname,
                        salaryAdjustmentValueObject.Rating,
                        "Rating is below the salary adjustment threshold"));
            }

            var salaryAdjustmentCriteriaValueObject = context.SalaryAdjustmentCriteriaValueObject;

            return PolicyResult.Success(
                new EmployeeSalaryAdjustmentAcceptedEvent(
                    salaryAdjustmentCriteriaValueObject.SalaryAdjustmentBaseRate,
                    salaryAdjustmentCriteriaValueObject.SalaryThreshold,
                    salaryAdjustmentCriteriaValueObject.BatchSize));
        }
    }
}