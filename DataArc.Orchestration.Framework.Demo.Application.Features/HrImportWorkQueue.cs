using DataArc.Orchestration.Framework.Demo.Application.Features.HR.EmployeeImports.Dtos;
using DataArc.Orchestration.Framework.Demo.Application.Features.HR.EmployeeImports.Services;

using System.Threading.Channels;

public sealed class HrImportWorkQueue : IHrImportWorkQueue
{
    private readonly Channel<HrImportWorkItem> _channel =
        Channel.CreateUnbounded<HrImportWorkItem>();

    public ValueTask EnqueueAsync(
        HrImportWorkItem workItem,
        CancellationToken cancellationToken = default)
    {
        return _channel.Writer.WriteAsync(workItem, cancellationToken);
    }

    public IAsyncEnumerable<HrImportWorkItem> ReadAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _channel.Reader.ReadAllAsync(cancellationToken);
    }
}