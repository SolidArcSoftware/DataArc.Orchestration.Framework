using Demo.Orchestration.HR.Orchestrators.Input;
using Demo.Orchestration.HR.Orchestrators.Ouput;
using Demo.Orchestration.HR.Orchestrators.Output;

namespace Demo.Orchestration.HR.Ports
{
    public interface IHROrchestrationPort
    {
        Task<PrepareEmployeeOnboardingOutput> PrepareEmployeeOnboardingAsync(PrepareEmployeeOnboardingInput input);
        Task<OnboardEmployeeOutput> OnboardEmployeeAsync(OnboardEmployeeInput input);
    }
}