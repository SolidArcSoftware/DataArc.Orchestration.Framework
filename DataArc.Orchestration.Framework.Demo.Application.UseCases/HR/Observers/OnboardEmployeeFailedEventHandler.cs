using DataArc.Observer;
using DataArc.Orchestration.Framework.Demo.Application.Domain.HR.Events;

namespace DataArc.Orchestration.Framework.Demo.Application.UseCases.HR.Observers
{
    public sealed class OnboardEmployeeRejectedEventHandler
       : IEventObserver<OnboardEmployeeRejectedEvent>
    {
        public Task HandleAsync(OnboardEmployeeRejectedEvent evt)
        {
            Console.WriteLine(
                $"[HR] Employee onboarding rejected. Reason: {evt.Reason}");

            return Task.CompletedTask;
        }
    }
}