using DataArc.Orchestration.Framework.Demo.Application.Domain.HR.Events;
using DataArc.Orchestration.Framework.Demo.Application.Domain.HR.Policies.Contexts;
using DataArc.Orchestration.Framework.Demo.Application.Domain.SharedKernel;

namespace DataArc.Orchestration.Framework.Demo.Application.Domain.HR.Policies
{
    internal class EmployeeOnboardingPolicy : IEmployeeOnboardingPolicy
    {
        public PolicyResult Apply(EmployeeOnboardingPoicyContext context)
        {
            var onboardingCriteriaValueObject = context.EmployeeOnboardingCriteriaValueObject;

            if (onboardingCriteriaValueObject.MeetsOnboardingCriteria) {
                return PolicyResult.Success(
                    new EmployeeOnboardingAcceptedEvent(
                        onboardingCriteriaValueObject.EmployeeId,
                        onboardingCriteriaValueObject.AnnualSalary,
                        onboardingCriteriaValueObject.CurrencyCode,
                        onboardingCriteriaValueObject.Reason,
                        onboardingCriteriaValueObject.EffectiveOnUtc));
            }

            return PolicyResult.Fail("Employee onboarding policy rejected", 
                new EmployeeOnboardingRejectedEvent());
        }
    }
}