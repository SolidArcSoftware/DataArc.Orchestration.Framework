using DataArc.Orchestrator;

namespace DataArc.Orchestration.Framework.Demo.Application.Modules.Finance.UseCases
{
    public record PrepareTopRatedEmployeesInput(double Rating) : IOrchestratorInput;
}