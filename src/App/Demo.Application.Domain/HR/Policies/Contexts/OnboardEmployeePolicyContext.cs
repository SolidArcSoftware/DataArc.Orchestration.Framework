using Demo.Application.Domain.HR.ValueObjects;

namespace Demo.Application.Domain.HR.Policies.Contexts
{
    public sealed class OnboardEmployeePolicyContext
    {
        public OnBoardEmployeeValueObject Employee { get; }

        public OnboardEmployeePolicyContext(
            OnBoardEmployeeValueObject employee)
        {
            Employee = employee;
        }
    }
}