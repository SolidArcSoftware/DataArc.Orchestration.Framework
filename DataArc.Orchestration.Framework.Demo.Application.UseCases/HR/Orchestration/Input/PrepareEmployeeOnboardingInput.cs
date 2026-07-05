using DataArc.Orchestrator;

namespace DataArc.Orchestration.Framework.Demo.Application.UseCases.HR.Orchestration.Input
{
    public record PrepareEmployeeOnboardingInput(int EmployeeId) : IOrchestratorInput { }
}