using DataArc.Orchestration.Framework.Demo.Orchestration.Orchestrators;
using DataArc.Orchestrator;
using Microsoft.Extensions.DependencyInjection;

namespace DataArc.Orchestration.Framework.Demo.Orchestration.Finance
{
    public static class FinanceOrchestrationRegistration
    {
        public static IServiceCollection AddFinanceOrchestration(this IServiceCollection services)
        {
            services.AddDataArcOrchestrator(orchestrators =>
            {
                orchestrators.Add<PrepareTopRatedEmployeesOrchestrator>();
                orchestrators.Add<ProcessEmployeeSalaryAdjustmentsOrchestrator>();
            });

            return services;
        }
    }
}