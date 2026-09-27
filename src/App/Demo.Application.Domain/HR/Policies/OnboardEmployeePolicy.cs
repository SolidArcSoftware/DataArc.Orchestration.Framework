using Demo.Application.Domain.HR.Events;
using Demo.Application.Domain.HR.Policies.Contexts;
using Demo.Application.Domain.SharedKernel;

namespace Demo.Application.Domain.HR.Policies
{
    public class OnboardEmployeePolicy : IEmployeeOnboardingPolicy
    {
        public PolicyResult Apply(OnboardEmployeePolicyContext context)
        {
            if (context == null || context.Employee == null)
            {
                throw new InvalidOperationException(
                    "Onboarding policy context was null");
            }

            if (context.Employee.IsOnboarded)
            {
                return PolicyResult.Fail(
                    "Employee onboarding cannot be completed because the employee has already been onboarded.",
                    new OnboardEmployeeRejectedEvent(
                        "The employee has already been onboarded"));
            }

            if (context.PayrollRecordExists ||
                context.AccessRequestExists ||
                context.OnboardingTaskExists)
            {
                return PolicyResult.Fail(
                    "Employee onboarding cannot be completed because onboarding records already exist.",
                    new OnboardEmployeeRejectedEvent(
                        "Existing employee onboarding records were found"));
            }

            return PolicyResult.Success(
                new OnboardEmployeeAcceptedEvent(
                    "Employee onboarding policy accepted the request."));
        }
    }
}