using Demo.Application.Features.HR.EmployeeImports.Dtos;

namespace Demo.Application.Features.HR.EmployeeImports.Services
{
    public interface IEmployeeImportsService
    {
        Task<ImportEmployeesResponseDto> ImportEmployeeData();
    }
}