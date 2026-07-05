using DataArc.Orchestrator;

namespace DataArc.Orchestration.Framework.Demo.Application.UseCases.HR.Orchestration.Ouput
{
    public class ImportEmployeesOutput : IOrchestratorOutput
    {
        public List<string>? Errors { get; set; }
        public int TotalRecordsProcessed { get; set; }
    }
}