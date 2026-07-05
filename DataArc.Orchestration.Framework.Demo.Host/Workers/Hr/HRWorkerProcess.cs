using DataArc.Orchestration.Framework.Demo.Application.Features.HR.EmployeeImports.Services;
using DataArc.Orchestration.Framework.Demo.Application.UseCases.HR.Orchestration.Input;
using DataArc.Orchestration.Framework.Demo.Application.UseCases.HR.Ports;
using Microsoft.Extensions.Hosting;

internal sealed class HrWorkerProcess : BackgroundService
{
    private readonly IHrImportWorkQueue _workQueue;
    private readonly IHROrchestrationPort _hrOrchestrationPort;

    public HrWorkerProcess(
        IHrImportWorkQueue workQueue,
        IHROrchestrationPort hrOrchestrationPort)
    {
        _workQueue = workQueue;
        _hrOrchestrationPort = hrOrchestrationPort;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await foreach (var workItem in _workQueue.ReadAllAsync(stoppingToken))
            {
                var output = await _hrOrchestrationPort.ImportEmployeesDataAsync(
                    new ImportEmployeesInput
                    {
                        ImportBatchSize = workItem.ImportBatchSize,
                        ImportEmployeeCount = workItem.ImportEmployeeCount
                    });

                if (output.Errors != null && output.Errors.Any())
                {
                    foreach (var error in output.Errors)
                    {
                        Console.WriteLine(error);
                    }

                    continue;
                }

                Console.WriteLine($"Total records processed ({output.TotalRecordsProcessed})");
            }
        }
    }
}