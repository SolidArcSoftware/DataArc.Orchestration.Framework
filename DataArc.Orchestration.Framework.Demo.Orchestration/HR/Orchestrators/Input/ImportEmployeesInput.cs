using DataArc.Orchestrator;

namespace DataArc.Orchestration.Framework.Demo.Orchestration.HR.Orchestrators.Input
{
    public class ImportEmployeesInput : IOrchestratorInput
    {
        public int ImportEmployeeCount { get; set; }
        public int ImportBatchSize { get; set; }
    }
}