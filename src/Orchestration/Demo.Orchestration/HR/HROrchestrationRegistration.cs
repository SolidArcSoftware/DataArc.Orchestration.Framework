using DataArc.Observer;
using Demo.Orchestration.HR.Observers;
using Demo.Orchestration.HR.Orchestrators;
using DataArc.Orchestrator;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Orchestration.HR
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