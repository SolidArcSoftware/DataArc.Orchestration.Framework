using Demo.Application.Domain.HR.ValueObjects;

namespace Demo.Application.Domain.HR.Policies.Contexts
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
