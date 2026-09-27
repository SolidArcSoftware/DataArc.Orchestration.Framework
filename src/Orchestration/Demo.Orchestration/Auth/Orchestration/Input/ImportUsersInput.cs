using DataArc.Orchestrator;

namespace Demo.Orchestration.Auth.Orchestration.Input
{
    public class ImportUsersInput : IOrchestratorInput
    {
        public int ImportCountCount { get; set; }
        public int ImportBatchSize { get; set; }
    }
}