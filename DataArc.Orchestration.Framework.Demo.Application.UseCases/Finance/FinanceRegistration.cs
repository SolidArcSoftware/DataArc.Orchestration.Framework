using Microsoft.Extensions.DependencyInjection;
using DataArc.Observer;

using DataArc.Orchestration.Framework.Demo.Application.UseCases.Finance.Observers;

namespace DataArc.Orchestration.Framework.Demo.Application.UseCases.Finance
{
    public static class FinanceRegistration
    {
        public static IServiceCollection AddFinanceUseCases(this IServiceCollection services) {
            
            services.AddDataArcObserver(observers =>
            {
                observers.Add<EmployeeSalaryAdjustmentAcceptedObserver>();
                observers.Add<EmployeeSalaryAdjustmentRejectedObserver>();
            });

            return services;
        }
    }
}