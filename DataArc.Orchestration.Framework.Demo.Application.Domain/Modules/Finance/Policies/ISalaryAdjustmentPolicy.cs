using DataArc.Demo.Domain.SharedKernel;
using DataArc.Orchestration.Framework.Demo.Application.Domain.Modules.Finance.Policies.Context;

namespace DataArc.Orchestration.Framework.Demo.Application.Domain.Modules.Finance.Policies
{
    public interface ISalaryAdjustmentPolicy : IPolicy<SalaryAdjustmentPolicyContext>
    {

    }
}