using Demo.Orchestration.HR.Orchestrators.Input;
using Demo.Orchestration.HR.Orchestrators.Ouput;

namespace Demo.Orchestration.HR.Ports
{
    public interface IHROrchestrationPort
    {
        Task<ImportEmployeesOutput> ImportEmployeesDataAsync(ImportEmployeesInput input);
        Task<OnboardEmployeeOutput> OnboardEmployeeAsync(OnboardEmployeeInput input);
    }
}