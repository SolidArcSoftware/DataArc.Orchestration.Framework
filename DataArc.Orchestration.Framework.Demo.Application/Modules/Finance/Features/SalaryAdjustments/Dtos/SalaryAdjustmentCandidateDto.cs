    namespace DataArc.Orchestration.Framework.Demo.Modules.Finance.Features.SalaryAdjustments.Dtos
    {
        public sealed class SalaryAdjustmentCandidateDto
        {
            public int EmployeeId { get; set; }
            public string? Name { get; set; }
            public string? Surname { get; set; }
            public decimal? CurrentSalary { get; set; }
        }
    }