using DataArc.Orchestration.Framework.Demo.Orchestration.HR.Orchestrators.Input;
using DataArc.Orchestration.Framework.Demo.Orchestration.HR.Orchestrators.Ouput;

namespace DataArc.Orchestration.Framework.Demo.Orchestration.HR.Ports
{
    public interface IHROrchestrationPort
    {
        Task<ImportEmployeesOutput> ImportEmployeesDataAsync(ImportEmployeesInput input);
        Task<OnboardEmployeeOutput> OnboardEmployeeAsync(OnboardEmployeeInput input);
    }
}