using DataArc.Orchestrator;

namespace DataArc.Orchestration.Framework.Demo.Application.UseCases.HR.Orchestration.Ouput
{
    public sealed class OnboardEmployeeOutput : IOrchestratorOutput
    {
        public bool IsSuccess { get; set; }
        public string? FailureReason { get; set; }
        public int PayrollRecordId { get; set; }
        public int EmployeePayrollRecordId { get; set; }
    }
}