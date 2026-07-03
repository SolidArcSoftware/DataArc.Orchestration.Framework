using Microsoft.Extensions.DependencyInjection;

using DataArc.Orchestration.Framework.Demo.Application.Domain.Modules.Finance.Policies;
using DataArc.Orchestration.Framework.Demo.Modules.Finance.Features.SalaryAdjustments.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DataArc.Orchestration.Framework.Demo.Application.Features.Finance
{
    public static class FinanceRegistration
    {
        public static IServiceCollection AddFinanceFeatures(this IServiceCollection services)
        {
            // Policies
            services.TryAddScoped<ISalaryAdjustmentPolicy, SalaryAdjustmentPolicy>();

            // Application Services
            services.TryAddScoped<ISalaryAdjustmentService, SalaryAdjustmentService>();

            return services;
        }
    }
}