using DataArc.Observer;
using Demo.Application.Domain.HR.Events;

namespace Demo.Orchestration.HR.Observers
{
    internal class OnboardEmployeeFailedEventObserver : IEventObserver<OnboardEmployeeRejectedEvent>
    {
        public Task HandleAsync(OnboardEmployeeRejectedEvent evt)
        {
            Console.WriteLine(
                $"[HR] Employee onboarding rejected. Reason: {evt.Reason}");

            return Task.CompletedTask;
        }
    }
}