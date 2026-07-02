using DataArc.Orchestration.Framework.Demo.Application.Modules.Finance.Features.SalaryAdjustments.Dtos;

namespace DataArc.Orchestration.Framework.Demo.Application.Modules.Finance.Features.SalaryAdjustments.Services
{
    public interface IEmployeePerformanceService
    {
        Task<List<EmployeeDto>> GetTopRatedEmployeesAsync(double rating);
    }
}