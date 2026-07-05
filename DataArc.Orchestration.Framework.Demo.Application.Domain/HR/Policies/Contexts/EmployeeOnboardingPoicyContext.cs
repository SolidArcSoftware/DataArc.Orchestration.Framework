using DataArc.Orchestration.Framework.Demo.Application.Domain.HR.ValueObjects;

namespace DataArc.Orchestration.Framework.Demo.Application.Domain.HR.Policies.Contexts
{
    internal class EmployeeOnboardingPoicyContext
    {
        public EmployeeOnboardingCriteriaValueObject EmployeeOnboardingCriteriaValueObject { get; set; }
        public EmployeeOnboardingPoicyContext(EmployeeOnboardingCriteriaValueObject employeeOnboardingCriteriaValueObject)
        {
            EmployeeOnboardingCriteriaValueObject = employeeOnboardingCriteriaValueObject;
        }
    }
}