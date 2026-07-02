using DataArc.Orchestration.Framework.Demo.Modules.Finance.Features.SalaryAdjustments.Dtos;

namespace DataArc.Orchestration.Framework.Demo.Modules.Finance.Features.SalaryAdjustments.Services
{
    public interface IEmployeePerformanceService
    {
        Task<List<SalaryAdjustmentCandidateDto>> GetTopRatedEmployeesAsync(double rating);
    }
}