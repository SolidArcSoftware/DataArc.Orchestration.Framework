using DataArc.Orchestrator;

namespace DataArc.Orchestration.Framework.Demo.Application.Modules.Finance.UseCases
{
    public sealed record PreparedTopRatedEmployee
    {
        public int EmployeeId { get; init; }
        public string? Name { get; init; }
        public string? Surname { get; init; }
        public decimal CurrentSalary { get; init; }
        public double? Rating { get; init; }
    }

    public sealed class PrepareTopRatedEmployeesOutput : IOrchestratorOutput
    {
        public List<PreparedTopRatedEmployee>? PrepareTopRatedEmployees { get; set; }
    }
}