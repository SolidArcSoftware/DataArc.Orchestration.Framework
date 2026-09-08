using DataArc.Orchestration.Framework.Demo.Application.Features.HR.EmployeeImports.Dtos;
using DataArc.Orchestration.Framework.Demo.Application.Features.HR.EmployeeImports.Services;
using DataArc.Orchestration.Framework.Demo.Orchestration.HR.Orchestrators.Input;
using DataArc.Orchestration.Framework.Demo.Orchestration.HR.Ports;

namespace DataArc.Orchestration.Framework.Demo.Application.Modules.HR.Services
{
    internal sealed class EmployeeImportsService : IEmployeeImportsService
    {
        readonly IHROrchestrationPort _hROrchestration;
        public EmployeeImportsService(IHROrchestrationPort hROrchestration)
        {
            _hROrchestration = hROrchestration;
        }

        public async Task<ImportEmployeesResponseDto> ImportEmployeeData()
        {
            var result = await _hROrchestration.ImportEmployeesDataAsync(new ImportEmployeesInput() {
                ImportEmployeeCount = 100_000,
                ImportBatchSize = 100_000,
            });

            return new ImportEmployeesResponseDto()
            {
                TotalRecordsProcessed = result.TotalRecordsProcessed,
                Errors = result.Errors ?? new List<string>()
            };
        }
    }
}