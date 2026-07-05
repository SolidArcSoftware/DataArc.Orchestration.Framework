using DataArc.Orchestration.Framework.Demo.Application.Features.HR.EmployeeImports.Dtos;
using DataArc.Orchestration.Framework.Demo.Application.UseCases.HR.Orchestration.Input;
using DataArc.Orchestration.Framework.Demo.Application.UseCases.HR.Ports;

namespace DataArc.Orchestration.Framework.Demo.Application.Features.HR.EmployeeImports.Services
{
    internal class EmployeeImportsService : IEmployeeImportsService
    {
        readonly IHROrchestrationPort _hROrchestrationPort;
        public EmployeeImportsService(IHROrchestrationPort hROrchestrationPort)
        {
            _hROrchestrationPort = hROrchestrationPort;
        }

        public async Task<ImportEmployeesResponseDto> ImportEmployeeData()
        {
            var result = await _hROrchestrationPort.ImportEmployeesDataAsync(new ImportEmployeesInput() {
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