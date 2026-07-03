namespace DataArc.Orchestration.Framework.Demo.Modules.Finance.Features.SalaryAdjustments.Dtos
{
    public record SalaryAdjustmentCandidateRequestDto
    {
        public double? Rating { get; set; }
        public decimal SalaryAdjustmentBaseRate { get; }
        public decimal SalaryThreshold { get; }
        public int BatchSize { get; }
    }
}