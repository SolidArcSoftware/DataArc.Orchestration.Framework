using Demo.Application.Features.HR.EmployeeImports.Dtos;

namespace Demo.Application.Features.HR.EmployeeImports.Services
{
    public interface IUserImportsService
    {
        Task<ImportUserssResponseDto> ImportUserData();
    }
}