using Demo.Application.Features.Auth.UserImports.Dtos;
using Demo.Application.Features.Auth.UserImports.Services;
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

        public async Task<ImportUsersResponseDto> ImportUserData()
        {
            var result = await _authOrchestration.ImportUsersDataAsync(new ImportUsersInput() {
                ImportCountCount = 100_000,
                ImportBatchSize = 100_000,
            });

            return new ImportUsersResponseDto()
            {
                TotalRecordsProcessed = result.TotalRecordsProcessed,
                Errors = result.Errors ?? new List<string>()
            };
        }
    }
}