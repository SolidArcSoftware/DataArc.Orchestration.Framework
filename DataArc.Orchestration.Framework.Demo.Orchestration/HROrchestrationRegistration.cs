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

            return services;
        }
    }
}