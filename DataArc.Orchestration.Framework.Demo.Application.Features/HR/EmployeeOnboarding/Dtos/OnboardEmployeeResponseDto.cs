namespace DataArc.Orchestration.Framework.Demo.Application.Features.HR.EmployeeOnboarding.Dtos
{
    public sealed class OnboardEmployeeResponseDto
    {
        public bool IsSuccess { get; set; }
        public string? FailureReason { get; set; }

        public int EmployeeId { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Status { get; set; }
        public double Rating { get; set; }
        public decimal Salary { get; set; }

        public int PayrollRecordId { get; set; }
        public int EmployeePayrollRecordId { get; set; }
    }
}