using DataArc.Orchestrator;
using Demo.Orchestration.Auth.Orchestration;
using Demo.Orchestration.Auth.Orchestration.Input;
using Demo.Orchestration.Auth.Orchestration.Output;
using Demo.Orchestration.Auth.Ports;

namespace Demo.Application.Modules.Auth.Adapters
{
    internal class AuthOrchestrationAdapter : IAuthOrchestrationPort
    {
        private readonly IOrchestrator _orchestrator;

        public AuthOrchestrationAdapter(IOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
        }

        public async Task<ImportUsersOutput> ImportUsersDataAsync(ImportUsersInput input)
            => await _orchestrator
                .OrchestrateAsync<ImportUsersDataOrchestrator, ImportUsersOutput>(
                    input,
                    new ImportUsersOutput());
    }
}