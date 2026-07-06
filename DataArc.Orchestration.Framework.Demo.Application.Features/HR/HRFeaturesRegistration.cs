using DataArc.Orchestration.Framework.Demo.Application.Features.HR.EmployeeImports.Services;
using DataArc.Orchestration.Framework.Demo.Application.Features.HR.EmployeeOnboarding.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DataArc.Orchestration.Framework.Demo.Application.Features.HR
{
    public static class HRFeaturesRegistration
    {
        public static IServiceCollection AddHRFeatures(this IServiceCollection services)
        {
            services.TryAddScoped<IEmployeeImportsService, EmployeeImportsService>();
            services.TryAddScoped<IEmployeeOnboardingService, EmployeeOnboardingService>();

            return services;
        }
    }
}
