using Demo.Application.Features.Auth.UserImports.Dtos;

namespace Demo.Application.Features.Auth.UserImports.Services
{
    public interface IUserImportsService
    {
        Task<ImportUsersResponseDto> ImportUserData();
    }
}