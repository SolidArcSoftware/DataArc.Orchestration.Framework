using DataArc.Observer;
using Microsoft.Extensions.DependencyInjection;

namespace DataArc.Orchestration.Framework.Demo.Application.UseCases.HR
{
    public static class HRRegistration
    {
        public static IServiceCollection AddHRUseCases(this IServiceCollection services) {
            services.AddDataArcObserver(observers =>
            {
                observers.Add<ImportEmployeeDataRequestAcceptedEventObserver>();
            });

            return services;
        }
    }
}
