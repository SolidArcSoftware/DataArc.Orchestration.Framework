using Demo.Application.Features.HR.EmployeeOnboarding.Dtos;
using Demo.Application.Features.HR.EmployeeOnboarding.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Integration.Tests.Modules.HR
{
    [TestFixture]
    [NonParallelizable]
    [TestFixture]
    internal class HRModuleIntegrationTests : SetupDbBase
    {
        [Test]
        public async Task OnboardEmployee_Should_Execute_Complete_Workflow()
        {
            // Arrange
            var employeeOnboardingService =
                ServiceProvider.GetRequiredService<IEmployeeOnboardingService>();

            var request = new OnboardEmployeeRequestDto
            {
                EmployeeId = EmployeeId,
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
                Assert.That(response.IsSuccess, Is.True, response.FailureReason);
                Assert.That(response.EmployeeId, Is.EqualTo(EmployeeId));
            });
        }
    }
}