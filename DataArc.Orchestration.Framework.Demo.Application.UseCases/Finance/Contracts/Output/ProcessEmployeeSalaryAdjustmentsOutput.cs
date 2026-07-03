using DataArc.Orchestrator;

namespace DataArc.Orchestration.Framework.Demo.Application.Modules.Finance.UseCases
{
    public class ProcessEmployeeSalaryAdjustmentsOutput : IOrchestratorOutput
    {
        public int TotalRecordsProcessed { get; set; }
        public ProcessEmployeeSalaryAdjustmentsOutput()
        {
            
        }
    }
}