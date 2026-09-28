using DataArc.Orchestrator;

namespace Demo.Orchestration.HR.Orchestrators.Input
{
    public sealed class PrepareEmployeeOnboardingInput : IOrchestratorInput
    {
        public string UserName { get; set; } = string.Empty;
    }
}