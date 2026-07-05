namespace DataArc.Orchestration.Framework.Demo.Application.Features.HR.EmployeeImports.Dtos
{
    public sealed class HrImportWorkItem
    {
        public int ImportBatchSize { get; init; }
        public int ImportEmployeeCount { get; init; }
    }
}
