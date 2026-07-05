using DataArc.Observer;
using DataArc.Orchestration.Framework.Demo.Application.Domain.HR.Policies;
using DataArc.Orchestration.Framework.Demo.Application.UseCases.HR.Observers;
using Microsoft.Extensions.DependencyInjection;

namespace DataArc.Orchestration.Framework.Demo.Application.UseCases.HR
{
    public static class HRRegistration
    {
        public static IServiceCollection AddHRUseCases(this IServiceCollection services) {
            services.AddDataArcObserver(observers =>
            {
                observers.Add<OnboardEmployeeRejectedEventHandler>();
                observers.Add<OnboardEmployeeAcceptedEventHandler>();
            });

            services.AddScoped<IEmployeeOnboardingPolicy, OnboardEmployeePolicy>();
            return services;
        }
    }
}