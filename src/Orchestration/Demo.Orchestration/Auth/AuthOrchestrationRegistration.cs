using Microsoft.Extensions.DependencyInjection;

using DataArc.Orchestrator;
using Demo.Orchestration.Auth.Orchestration;

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