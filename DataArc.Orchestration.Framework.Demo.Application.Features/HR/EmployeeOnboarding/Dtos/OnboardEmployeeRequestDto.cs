namespace DataArc.Orchestration.Framework.Demo.Application.Features.HR.EmployeeOnboarding.Dtos
{
    public sealed class OnboardEmployeeRequestDto
    {
        public int EmployeeId { get; set; } = 1;
        public decimal AnnualSalary { get; set; } = 85_000;
        public string CurrencyCode { get; set; } = "USD";
        public string Reason { get; set; } = "Demo employee onboarding";
    }
}