using DataArc.Orchestration.Framework.Demo.Application.Features.HR.EmployeeImports.Dtos;

namespace DataArc.Orchestration.Framework.Demo.Application.Features.HR.EmployeeImports.Services
{
    public interface IEmployeeImportsService
    {
        Task<ImportEmployeesResponseDto> ImportEmployeeData();
    }
}