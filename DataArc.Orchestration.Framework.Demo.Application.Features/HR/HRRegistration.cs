using DataArc.Orchestration.Framework.Demo.Application.Features.HR.EmployeeImports.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DataArc.Orchestration.Framework.Demo.Application.Features.HR
{
    public static class HRRegistration
    {
        public static IServiceCollection AddHRFeatures(this IServiceCollection services)
        {
            services.TryAddScoped<IEmployeeImportsService, EmployeeImportsService>();

            return services;
        }
    }
}
