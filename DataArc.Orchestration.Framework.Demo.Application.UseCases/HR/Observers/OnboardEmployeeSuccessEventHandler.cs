using DataArc.Observer;
using DataArc.Orchestration.Framework.Demo.Application.Domain.HR.Events;

namespace DataArc.Orchestration.Framework.Demo.Application.UseCases.HR.Observers
{
    public sealed class OnboardEmployeeAcceptedEventHandler
        : IEventObserver<OnboardEmployeeAcceptedEvent>
    {
        public Task HandleAsync(OnboardEmployeeAcceptedEvent evt)
        {
            Console.WriteLine(
                $"[HR] Employee onboarding accepted by policy. Reason: {evt.Reason}.");

            return Task.CompletedTask;
        }
    }
}