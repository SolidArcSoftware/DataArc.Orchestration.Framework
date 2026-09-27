using Demo.Application.Domain.HR.ValueObjects;

namespace Demo.Application.Domain.HR.Policies.Contexts
{
    public sealed class OnboardEmployeePolicyContext
    {
        public OnBoardEmployeeValueObject Employee { get; }

        public bool HasDepartment { get; }

        public bool PayrollRecordExists { get; }

        public bool AccessRequestExists { get; }

        public bool OnboardingTaskExists { get; }

        public OnboardEmployeePolicyContext(
            OnBoardEmployeeValueObject employee,
            bool hasDepartment,
            bool payrollRecordExists,
            bool accessRequestExists,
            bool onboardingTaskExists)
        {
            Employee = employee;
            HasDepartment = hasDepartment;
            PayrollRecordExists = payrollRecordExists;
            AccessRequestExists = accessRequestExists;
            OnboardingTaskExists = onboardingTaskExists;
        }
    }
}