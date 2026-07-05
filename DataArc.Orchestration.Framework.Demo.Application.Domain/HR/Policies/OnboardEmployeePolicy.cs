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

            List<IDomainEvent> domainEvents = new List<IDomainEvent>();

            var employee = context.OnBoardEmployeeValueObject;

            if (!employee.IsActive)
                domainEvents.Add(new OnboardEmployeeRejectedEvent("Only active employees can be onboarded."));

            if (employee.HasPayrollRecord)
                domainEvents.Add(new OnboardEmployeeRejectedEvent("Employee already has a payroll record."));

            if (employee.HasAccessRequest)
                domainEvents.Add(new OnboardEmployeeRejectedEvent("Employee already has an access request."));

            if (employee.HasOnboardingTask)
                domainEvents.Add(new OnboardEmployeeRejectedEvent("Employee already has an onboarding task."));

            if (domainEvents.Count > 0)
            {
                return PolicyResult.Fail(
                    "Employee onboarding policy rejected the request.",
                    domainEvents.ToArray());
            }

            return PolicyResult.Success(
                new OnboardEmployeeAcceptedEvent("Employee onboarding policy accepted the request."));
        }
    }
}