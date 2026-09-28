using Demo.Application.Features.HR.EmployeeOnboarding.Dtos;
using Demo.Application.Features.HR.EmployeeOnboarding.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Integration.Tests.Modules.HR
{
    [TestFixture]
    [NonParallelizable]
    internal class HRModuleIntegrationTests : SetupModulesBase
    // Setting up all modules for demo purposes
    {
        [Test]
        public async Task OnboardEmployee_Should_Execute_Complete_Workflow()
        {
            // Arrange
            var employeeOnboardingService =
                ServiceProvider.GetRequiredService<IEmployeeOnboardingService>();

            var request = new OnboardEmployeeRequestDto
            {
                UserName = "INTEGRATION.EMPLOYEE@SOLIDARCSOFTWARE.COM",
                AnnualSalary = 95_000,
                CurrencyCode = "USD",
                Reason = "Integration test employee onboarding"
            };

            // Act
            var response =
                await employeeOnboardingService.OnboardEmployeeAsync(request);

            // Assert
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