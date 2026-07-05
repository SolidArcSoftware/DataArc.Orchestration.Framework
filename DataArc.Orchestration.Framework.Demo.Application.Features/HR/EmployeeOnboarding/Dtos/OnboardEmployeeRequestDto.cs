namespace DataArc.Orchestration.Framework.Demo.Application.Features.HR.EmployeeOnboarding.Dtos
{
    public sealed class OnboardEmployeeRequestDto
    {
        public int EmployeeId { get; set; }
        public decimal AnnualSalary { get; set; }
        public string CurrencyCode { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public DateTime EffectiveOnUtc { get; set; }
    }
}