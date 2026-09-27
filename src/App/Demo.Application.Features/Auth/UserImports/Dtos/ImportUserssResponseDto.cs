namespace Demo.Application.Features.HR.EmployeeImports.Dtos
{
    public class ImportUserssResponseDto
    {
        public int TotalRecordsProcessed { get; set; }
        public List<string>? Errors { get; set; }
    }
}
