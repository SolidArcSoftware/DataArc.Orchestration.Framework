using DataArc.Orchestrator;

namespace Demo.Orchestration.HR.Orchestrators.Output
{
    public sealed class PrepareEmployeeOnboardingOutput : IOrchestratorOutput
    {
        // Identity
        public bool UserExists { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? EmailAddress { get; set; }
        // HR
        public bool EmployeeExists { get; set; }
        public int? EmployeeId { get; set; }
        public string? OnBoardingStatus { get; set; }
        public bool IsArchived { get; set; }
        public bool HasDepartment { get; set; }
        // Downstream onboarding state
        public bool PayrollRecordExists { get; set; }
        public bool AccessRequestExists { get; set; }
        public bool OnboardingTaskExists { get; set; }
    }
}