namespace Demo.Application.Features.HR.EmployeeImports.Dtos
{
    public class ImportEmployeesResponseDto
    {
        public int TotalRecordsProcessed { get; set; }
        public List<string>? Errors { get; set; }
    }
}
