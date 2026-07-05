using DataArc.Orchestration.Framework.Demo.Application.Domain.HR.ValueObjects;

namespace DataArc.Orchestration.Framework.Demo.Application.Domain.HR.Policies.Contexts
{
    public class OnboardEmployeePolicyContext
    {
        public OnBoardEmployeeValueObject OnBoardEmployeeValueObject { get; }
        public OnboardEmployeePolicyContext(OnBoardEmployeeValueObject onBoardEmployeeValueObject)
        {
            OnBoardEmployeeValueObject = onBoardEmployeeValueObject;
        }
    }
}
