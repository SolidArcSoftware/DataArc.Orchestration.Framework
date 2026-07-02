using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using DataArc.Orchestration.Framework.Demo.Modules.Finance.Features.SalaryAdjustments.Services;
using DataArc.Orchestration.Framework.Demo.Persistence;

namespace DataArc.Orchestration.Framework.Demo.Application.Registration
{
    public static class FinanceRegistrationModule
    {
        public static IServiceCollection AddFinanceModule(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            // Register persistence layer for Finance module
            services.AddPersistence(configurationManager);
            // Register Services
            services.AddScoped<ISalaryAdjustmentService, SalaryAdjustmentService>();
            services.AddScoped<IEmployeePerformanceService, EmployeePerformanceService>();
            return services;
        }
    }
}