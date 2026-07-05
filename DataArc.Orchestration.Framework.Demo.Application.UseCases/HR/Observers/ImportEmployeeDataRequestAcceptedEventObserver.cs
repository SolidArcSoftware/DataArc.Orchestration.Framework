using DataArc.Observer;
using DataArc.Orchestration.Framework.Demo.Application.Domain.HR.Events;
using DataArc.Orchestration.Framework.Demo.Application.Features.HR.EmployeeImports.Dtos;
using DataArc.Orchestration.Framework.Demo.Application.Features.HR.EmployeeImports.Services;

internal class ImportEmployeeDataRequestAcceptedEventObserver
    : IEventObserver<ImportEmployeeDataRequestAcceptedEvent>
{
    private readonly IHrImportWorkQueue _workQueue;

    public ImportEmployeeDataRequestAcceptedEventObserver(IHrImportWorkQueue workQueue)
    {
        _workQueue = workQueue;
    }

    public async Task HandleAsync(ImportEmployeeDataRequestAcceptedEvent evt)
    {
        await _workQueue.EnqueueAsync(new HrImportWorkItem
        {
            ImportBatchSize = evt.ImportBatchSize,
            ImportEmployeeCount = evt.ImportEmployeeCount
        });
    }
}