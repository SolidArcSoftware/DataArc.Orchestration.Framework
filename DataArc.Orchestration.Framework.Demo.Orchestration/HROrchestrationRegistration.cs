using DataArc.Observer;
using DataArc.Orchestration.Framework.Demo.Orchestration.HR.Observers;
using DataArc.Orchestration.Framework.Demo.Orchestration.HR.Orchestrators;
using DataArc.Orchestrator;
using Microsoft.Extensions.DependencyInjection;

namespace DataArc.Orchestration.Framework.Demo.Orchestration
{
    public static class HROrchestrationRegistration
    {
        public static IServiceCollection AddHROrchestration(this IServiceCollection services)
        {
            services.AddDataArcOrchestrator(orchestrators => {
                orchestrators.Add<ImportEmployeesDataOrchestrator>();
                orchestrators.Add<OnboardEmployeeOrchestrator>();
            });

            services.AddDataArcObserver(observers => {
                observers.Add<OnboardEmployeeFailedEventObserver>();
                observers.Add<OnboardEmployeeSuccessEventObserver>();
            });

            return services;
        }
    }
}