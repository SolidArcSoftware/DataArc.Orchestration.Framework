using DataArc.Orchestrator;

namespace Demo.Orchestration.Auth.Orchestration.Output
{
    public class ImportUsersOutput : IOrchestratorOutput
    {
        public List<string>? Errors { get; set; }
        public int TotalRecordsProcessed { get; set; }
    }
}