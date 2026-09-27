using DataArc.Orchestrator;
using Demo.Orchestration.Auth.Orchestration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Orchestration.Auth
{
    public static class AuthOrchestrationRegistration
    {
        public static IServiceCollection AddAuthOrchestration(this IServiceCollection services)
        {
            services.AddDataArcOrchestrator(orchestrators => {
                orchestrators.Add<ImportUsersDataOrchestrator>();
            });

            return services;
        }
    }
}