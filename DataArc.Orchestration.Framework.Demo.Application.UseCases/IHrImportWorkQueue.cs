using DataArc.Orchestration.Framework.Demo.Application.Features.HR.EmployeeImports.Dtos;

namespace DataArc.Orchestration.Framework.Demo.Application.Features.HR.EmployeeImports.Services
{
    public interface IHrImportWorkQueue
    {
        ValueTask EnqueueAsync(HrImportWorkItem workItem, CancellationToken cancellationToken = default);
        IAsyncEnumerable<HrImportWorkItem> ReadAllAsync(CancellationToken cancellationToken = default);
    }
}