namespace Demo.Application.Features.HR.EmployeeOnboarding.Dtos
{
    public class OnboardEmployeeCandidateDto
    {
        public bool IsSuccess { get; set; }
        public string? FailureReason { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeNameSurname { get; set; }
        public string? Status { get; set; }
        public double Rating { get; set; }
        public decimal EmployeeSalary { get; set; }
        public int? PayrollRecordId { get; set; }
        public decimal? PayrollAnnualSalary { get; set; }
        public string? PayrollCurrencyCode { get; set; }
        public bool? PayrollIsActive { get; set; }
        public int? AccessRequestId { get; set; }
        public string? AccessLevel { get; set; }
        public string? AccessRequestStatus { get; set; }
        public DateTimeOffset? AccessRequestedOnUtc { get; set; }
        public DateTimeOffset? AccessCompletedOnUtc { get; set; }
        public int? OnboardingTaskId { get; set; }
        public string? OnboardingTaskName { get; set; }
        public string? OnboardingTaskStatus { get; set; }
        public DateTimeOffset? OnboardingTaskCreatedOnUtc { get; set; }
        public DateTimeOffset? OnboardingTaskDueDateUtc { get; set; }
        public DateTimeOffset? OnboardingTaskCompletedOnUtc { get; set; }
    }
}