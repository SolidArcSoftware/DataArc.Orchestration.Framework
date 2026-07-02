namespace DataArc.Orchestration.Framework.Demo.Application.Domain.Modules.Finance.ValueObjects
{
    public sealed class SalaryAdjustmentValueObject
    {
        const decimal MinimumSalaryThreshold = 100_000m;
        const decimal MaximumSalaryThreshold = 200_000m;
        const decimal AdjustmentPercentage = 0.10m;

        public int EmployeeId { get; }
        public string? Name { get; }
        public string? Surname { get; }
        public decimal CurrentSalary { get; }
        public decimal AdjustedSalary { get; }
        public decimal AdjustmentAmount { get; }
        public double? Rating { get; }

        public SalaryAdjustmentValueObject(
            int employeeId,
            string? name,
            string? surname,
            decimal currentSalary,
            double? rating)
        {
            EmployeeId = employeeId;
            Name = name;
            Surname = surname;
            CurrentSalary = currentSalary;
            Rating = rating;

            AdjustmentAmount = CalculateAdjustmentAmount(currentSalary);
            AdjustedSalary = currentSalary + AdjustmentAmount;
        }

        public bool IsQualifiedForAdjustment =>
            CurrentSalary >= MinimumSalaryThreshold &&
            CurrentSalary <= MaximumSalaryThreshold;

        public string QualificationReason =>
            IsQualifiedForAdjustment
                ? "Employee salary is within the salary adjustment threshold."
                : "Employee salary is outside the salary adjustment threshold.";

        static decimal CalculateAdjustmentAmount(decimal currentSalary)
        {
            return decimal.Round(currentSalary * AdjustmentPercentage, 2);
        }
    }
}