using DataArc.Observer;
using Demo.Application.Domain.HR.Events;

namespace Demo.Orchestration.HR.Observers
{
    internal class OnboardEmployeeSuccessEventObserver : IEventObserver<OnboardEmployeeAcceptedEvent>
    {
        public Task HandleAsync(OnboardEmployeeAcceptedEvent evt)
        {
            Console.WriteLine(
                $"[HR] Employee onboarding accepted by policy. Reason: {evt.Reason}.");

            return Task.CompletedTask;
        }
    }
}
