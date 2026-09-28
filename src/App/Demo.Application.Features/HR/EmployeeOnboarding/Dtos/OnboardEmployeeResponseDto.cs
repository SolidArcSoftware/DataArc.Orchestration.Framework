namespace Demo.Application.Features.HR.EmployeeOnboarding.Dtos
{
    public sealed class OnboardEmployeeResponseDto
    {
        public string? FailureReason { get; set; }
        public int EmployeeId { get; set; }
        public int PayrollRecordId { get; set; }
    }
}