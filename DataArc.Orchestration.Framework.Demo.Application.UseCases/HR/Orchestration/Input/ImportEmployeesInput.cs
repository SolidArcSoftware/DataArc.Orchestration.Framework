using DataArc.Orchestrator;

namespace DataArc.Orchestration.Framework.Demo.Application.UseCases.HR.Orchestration.Input
{
    public class ImportEmployeesInput : IOrchestratorInput
    {
        public int ImportEmployeeCount { get; set; }
        public int ImportBatchSize { get; set; }
    }
}