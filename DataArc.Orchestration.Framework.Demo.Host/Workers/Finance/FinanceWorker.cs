using Microsoft.Extensions.Hosting;

using DataArc.Orchestration.Framework.Demo.Application.Domain.Modules.Finance.Events;
using DataArc.Orchestration.Framework.Demo.Application.Modules.Finance.UseCases;
using DataArc.Orchestration.Framework.Demo.Application.UseCases.Modules.Finance.Ports;

using DataArc.Observer;

internal sealed class FinanceWorker :
    BackgroundService,
    IEventObserver<EmployeeSalaryAdjustmentAcceptedEvent>
{
    private readonly IFinanceOrchestrationPort _financeOrchestrationPort;

    public FinanceWorker(
        IFinanceOrchestrationPort financeOrchestrationPort)
    {
        _financeOrchestrationPort = financeOrchestrationPort;
    }

    public async Task HandleAsync(EmployeeSalaryAdjustmentAcceptedEvent evt)
    {
        await _financeOrchestrationPort.ProcessEmployeeSalaryAdjustmentAsync(
            new ProcessEmployeeSalaryAdjustmentsInput(
                evt.salaryAdjustmentBaseRate, evt.salaryThreshold, evt.batchSize));
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }
}