namespace Demo.Application.Features.Auth.UserImports.Dtos
{
    public class ImportUsersResponseDto
    {
        public int TotalRecordsProcessed { get; set; }
        public List<string>? Errors { get; set; }
    }
}
