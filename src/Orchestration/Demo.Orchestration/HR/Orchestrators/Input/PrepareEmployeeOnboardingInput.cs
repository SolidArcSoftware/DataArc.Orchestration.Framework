using DataArc.Orchestrator;

namespace Demo.Orchestration.HR.Orchestrators.Input
{
    public sealed class PrepareEmployeeOnboardingInput : IOrchestratorInput
    {
        public int UserId { get; set; }
    }
}