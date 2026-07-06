using DataArc.Orchestration.Framework.Demo.Application.UseCases.HR.Orchestration.Input;
using DataArc.Orchestration.Framework.Demo.Application.UseCases.HR.Orchestration.Ouput;
using DataArc.Orchestration.Framework.Demo.Application.UseCases.HR.Ports;
using DataArc.Orchestration.Framework.Demo.Orchestration.HR.Orchestrators;

using DataArc.Orchestrator;

namespace DataArc.Orchestration.Framework.Demo.Application.Modules.HR.Adapters
{
    internal class HROrchestrationAdapter : IHROrchestration
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