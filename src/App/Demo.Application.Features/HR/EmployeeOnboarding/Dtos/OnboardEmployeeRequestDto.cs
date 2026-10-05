namespace Demo.Application.Features.HR.EmployeeOnboarding.Dtos
{
    public sealed class OnboardEmployeeRequestDto
    {
        public string UserName { get; set; } = string.Empty;

        public decimal AnnualSalary { get; set; }

        public string CurrencyCode { get; set; } = string.Empty;

        public string Reason { get; set; } = string.Empty;
    }
}