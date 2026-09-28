using Demo.Application.Features.HR.EmployeeOnboarding.Dtos;
using Demo.Application.Features.HR.EmployeeOnboarding.Services;

using Demo.Integration.Tests.Database;

using Microsoft.Extensions.DependencyInjection;

namespace Demo.Integration.Tests.Modules.HR
{
    [TestFixture]
    [NonParallelizable]
    internal sealed class HRModuleIntegrationTests : DemoIntegrationTestBase
    {
        [Test]
        public async Task OnboardEmployee_Should_Execute_Complete_Workflow()
        {
            // HR owns the onboarding use case, while the real workflow composes
            // Auth, HR, Finance, IT and Operations through the orchestration layer.
            var employeeOnboardingService =
                ServiceProvider.GetRequiredService<IEmployeeOnboardingService>();

            var request = new OnboardEmployeeRequestDto
            {
                UserName = "INTEGRATION.EMPLOYEE@SOLIDARCSOFTWARE.COM",
                AnnualSalary = 95_000,
                CurrencyCode = "USD",
                Reason = "Integration test employee onboarding"
            };

            var response =
                await employeeOnboardingService.OnboardEmployeeAsync(request);

            Assert.Multiple(() =>
            {
                Assert.That(
                    response.EmployeeId,
                    Is.GreaterThan(0));

                Assert.That(
                    response.PayrollRecordId,
                    Is.GreaterThan(0));
            });
        }
    }
}