using DataArc.Orchestration.Framework.Demo.Application.Domain.HR.Events;
using DataArc.Orchestration.Framework.Demo.Application.Domain.HR.Policies.Contexts;
using DataArc.Orchestration.Framework.Demo.Application.Domain.SharedKernel;

namespace DataArc.Orchestration.Framework.Demo.Application.Domain.HR.Policies
{
    public class OnboardEmployeePolicy : IEmployeeOnboardingPolicy
    {
        public PolicyResult Apply(OnboardEmployeePolicyContext context)
        {
            if (context == null || context.OnBoardEmployeeValueObject == null)
                throw new InvalidOperationException("Onboarding policy context was null");

            var employee = context.OnBoardEmployeeValueObject;

            if (employee.IsOnboarded)
            {
                return PolicyResult.Fail(
                    "Employee onboarding policy rejected the request.",
                    new OnboardEmployeeRejectedEvent("The employee has already been onboarded"));
            }

            return PolicyResult.Success(
                new OnboardEmployeeAcceptedEvent("Employee onboarding policy accepted the request."));
        }
    }
}