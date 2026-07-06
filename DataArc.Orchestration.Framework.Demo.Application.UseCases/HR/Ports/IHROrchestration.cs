using DataArc.Orchestration.Framework.Demo.Application.UseCases.HR.Orchestration.Input;
using DataArc.Orchestration.Framework.Demo.Application.UseCases.HR.Orchestration.Ouput;

namespace DataArc.Orchestration.Framework.Demo.Application.UseCases.HR.Ports
{
    public interface IHROrchestration
    {
        Task<ImportEmployeesOutput> ImportEmployeesDataAsync(ImportEmployeesInput input);
        Task<OnboardEmployeeOutput> OnboardEmployeeAsync(OnboardEmployeeInput input);
    }
}