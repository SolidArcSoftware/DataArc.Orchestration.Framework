namespace Demo.Application.Features.HR.EmployeeOnboarding.Dtos
{
    public sealed class OnboardEmployeeRequestDto
    {
        public int UserId { get; set; }
        public decimal AnnualSalary { get; set; }
        public string CurrencyCode { get; set; }
        public string Reason { get; set; }
    }
}