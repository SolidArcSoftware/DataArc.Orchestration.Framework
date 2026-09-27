using Demo.Application.Features.HR.EmployeeImports.Dtos;
using Demo.Application.Features.HR.EmployeeImports.Services;
using Demo.Orchestration.Auth.Orchestration.Input;
using Demo.Orchestration.Auth.Ports;

namespace Demo.Application.Modules.Auth.Services
{
    internal sealed class UserImportsService : IUserImportsService
    {
        readonly IAuthOrchestrationPort _authOrchestration;
        public UserImportsService(IAuthOrchestrationPort authOrchestration)
        {
            _authOrchestration = authOrchestration;
        }

        public async Task<ImportUserssResponseDto> ImportUserData()
        {
            var result = await _authOrchestration.ImportUsersDataAsync(new ImportUsersInput() {
                ImportCountCount = 100_000,
                ImportBatchSize = 100_000,
            });

            return new ImportUserssResponseDto()
            {
                TotalRecordsProcessed = result.TotalRecordsProcessed,
                Errors = result.Errors ?? new List<string>()
            };
        }
    }
}