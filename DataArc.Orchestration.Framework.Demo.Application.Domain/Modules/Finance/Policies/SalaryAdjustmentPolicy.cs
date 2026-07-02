using DataArc.Demo.Domain.SharedKernel;
using DataArc.Orchestration.Framework.Demo.Application.Domain.Modules.Finance.Events;
using DataArc.Orchestration.Framework.Demo.Application.Domain.Modules.Finance.Policies.Context;

namespace DataArc.Orchestration.Framework.Demo.Application.Domain.Modules.Finance.Policies
{
    internal sealed class SalaryAdjustmentPolicy : ISalaryAdjustmentPolicy
    {
        public PolicyResult Apply(SalaryAdjustmentPolicyContext context)
        {
            var valueObject = context.SalaryAdjustmentValueObject;

            if (!valueObject.IsQualifiedForAdjustment)
            {
                return PolicyResult.Fail(
                    "The employee is not qualified for a salary adjustment",
                    new RejectEmployeeSalaryAdjustmentEvent(
                        valueObject.EmployeeId,
                        valueObject.Name,
                        valueObject.Surname,
                        valueObject.Rating,
                        "Rating is below the salary adjustment threshold"));
            }

            return PolicyResult.Success(
                new AcceptEmployeeSalaryAdjustmentEvent(
                    valueObject.EmployeeId,
                    valueObject.Name,
                    valueObject.Surname,
                    valueObject.CurrentSalary,
                    valueObject.AdjustedSalary,
                    valueObject.Rating));
        }
    }
}