using DataArc.Orchestrator;

namespace Demo.Orchestration.HR.Orchestrators.Ouput
{
    public sealed class OnboardEmployeeOutput : IOrchestratorOutput
    {
        public int EmployeeId { get; set; }
        public int PayrollRecordId { get; set; }
        public bool IsSuccess { get; set; }
        public string? FailureReason { get; set; }
    }
}