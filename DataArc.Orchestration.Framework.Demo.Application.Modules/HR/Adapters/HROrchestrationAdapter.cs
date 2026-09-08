using DataArc.Orchestration.Framework.Demo.Orchestration.HR.Orchestrators;
using DataArc.Orchestration.Framework.Demo.Orchestration.HR.Orchestrators.Input;
using DataArc.Orchestration.Framework.Demo.Orchestration.HR.Orchestrators.Ouput;
using DataArc.Orchestration.Framework.Demo.Orchestration.HR.Ports;
using DataArc.Orchestrator;

namespace DataArc.Orchestration.Framework.Demo.Application.Modules.HR.Adapters
{
    internal class HROrchestrationAdapter : IHROrchestrationPort
    {
        private readonly IOrchestrator _orchestrator;
        public HROrchestrationAdapter(IOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
        }

        public async Task<OnboardEmployeeOutput> OnboardEmployeeAsync(OnboardEmployeeInput onBoardEmployeeInput)
            => await _orchestrator
                .OrchestrateAsync<OnboardEmployeeOrchestrator, OnboardEmployeeOutput>(
                    onBoardEmployeeInput,
                    new OnboardEmployeeOutput());

        public async Task<ImportEmployeesOutput> ImportEmployeesDataAsync(ImportEmployeesInput input) 
            => await _orchestrator
                .OrchestrateAsync<ImportEmployeesDataOrchestrator, ImportEmployeesOutput>(
                    input,
                    new ImportEmployeesOutput());
    }
}