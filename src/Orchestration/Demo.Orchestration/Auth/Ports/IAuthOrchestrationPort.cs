using Demo.Orchestration.Auth.Orchestration.Input;
using Demo.Orchestration.Auth.Orchestration.Output;

namespace Demo.Orchestration.Auth.Ports
{
    public interface IAuthOrchestrationPort
    {
        Task<ImportUsersOutput> ImportUsersDataAsync(ImportUsersInput input);
    }
}
