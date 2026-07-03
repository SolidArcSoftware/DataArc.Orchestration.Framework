using DataArc.Orchestration.Framework.Demo.Application.Modules.Finance.UseCases;
using DataArc.Orchestration.Framework.Demo.Application.UseCases.Modules.Finance.Ports;
using DataArc.Orchestration.Framework.Demo.Orchestration.Orchestrators;

using DataArc.Orchestrator;

namespace DataArc.Orchestration.Framework.Demo.Application.Modules.Finance.Adapters
{
    internal sealed class FinanceOrchestrationAdapter : IFinanceOrchestrationPort
    {
        private readonly IOrchestrator _orchestrator;

        public FinanceOrchestrationAdapter(IOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
        }

        public async Task<PrepareTopRatedEmployeesOutput> PrepareTopRatedEmployeesAsync(
            PrepareTopRatedEmployeesInput input,
            CancellationToken cancellationToken = default) => await _orchestrator.OrchestrateAsync<PrepareTopRatedEmployeesOrchestrator,
                PrepareTopRatedEmployeesOutput>(input,
                        new PrepareTopRatedEmployeesOutput());

        public async Task<ProcessEmployeeSalaryAdjustmentsOutput> ProcessEmployeeSalaryAdjustmentAsync(
            ProcessEmployeeSalaryAdjustmentsInput input,
            CancellationToken cancellationToken = default) => await _orchestrator.OrchestrateAsync<ProcessEmployeeSalaryAdjustmentsOrchestrator, 
                ProcessEmployeeSalaryAdjustmentsOutput>(input, 
                    new ProcessEmployeeSalaryAdjustmentsOutput());
    }
}