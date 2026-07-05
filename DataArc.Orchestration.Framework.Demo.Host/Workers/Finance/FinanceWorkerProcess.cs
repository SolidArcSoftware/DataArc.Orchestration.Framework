using DataArc.Observer;
using DataArc.Orchestration.Framework.Demo.Application.Domain.HR.Events;
using DataArc.Orchestration.Framework.Demo.Application.UseCases.Modules.Finance.Ports;
using Microsoft.Extensions.Hosting;

internal sealed class FinanceWorkerProcess
    : BackgroundService, IEventObserver<ImportEmployeeDataRequestAcceptedEvent>
{
    private readonly IFinanceOrchestrationPort _financeOrchestrationPort;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;

    public FinanceWorkerProcess(
       IFinanceOrchestrationPort financeOrchestrationPort, IHostApplicationLifetime hostApplicationLifetime)
    {
        _financeOrchestrationPort = financeOrchestrationPort;
        _hostApplicationLifetime = hostApplicationLifetime;
    }

    public Task HandleAsync(ImportEmployeeDataRequestAcceptedEvent evt)
    {
        throw new NotImplementedException();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {

        //_hostApplicationLifetime.StopApplication();
        return Task.CompletedTask;
    }
}