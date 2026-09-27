using DataArc.Orchestrator;

using Demo.Orchestration.HR.Orchestrators;
using Demo.Orchestration.HR.Orchestrators.Input;
using Demo.Orchestration.HR.Orchestrators.Ouput;
using Demo.Orchestration.HR.Orchestrators.Output;
using Demo.Orchestration.HR.Ports;

namespace Demo.Application.Modules.Modules.HR.Adapters
{
    internal class HROrchestrationAdapter : IHROrchestrationPort
    {
        private readonly IOrchestrator _orchestrator;
        public HROrchestrationAdapter(IOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
        }

        public async Task<PrepareEmployeeOnboardingOutput> PrepareEmployeeOnboardingAsync(PrepareEmployeeOnboardingInput input) 
            => await _orchestrator.OrchestrateAsync<
                PrepareEmployeeOnboardingOrchestrator, 
                    PrepareEmployeeOnboardingOutput>(input, new PrepareEmployeeOnboardingOutput());

        public async Task<OnboardEmployeeOutput> OnboardEmployeeAsync(OnboardEmployeeInput input) 
            => await _orchestrator.OrchestrateAsync<
                    OnboardEmployeeOrchestrator, OnboardEmployeeOutput>(input, new OnboardEmployeeOutput());
    }
}