using Microsoft.Extensions.DependencyInjection;

using DataArc.Observer;
using DataArc.Orchestrator;

using Demo.Orchestration.HR.Observers;
using Demo.Orchestration.HR.Orchestrators;

namespace Demo.Orchestration.HR
{
    public static class HROrchestrationRegistration
    {
        public static IServiceCollection AddHROrchestration(this IServiceCollection services)
        {
            services.AddDataArcOrchestrator(orchestrators =>
            {
                orchestrators.Add<PrepareEmployeeOnboardingOrchestrator>();
                orchestrators.Add<OnboardEmployeeOrchestrator>();
            });

            services.AddDataArcObserver(observers => 
            {
                observers.Add<OnboardEmployeeRejectedEventObserver>();
                observers.Add<OnboardEmployeeAcceptedEventObserver>();
            });

            return services;
        }
    }
}