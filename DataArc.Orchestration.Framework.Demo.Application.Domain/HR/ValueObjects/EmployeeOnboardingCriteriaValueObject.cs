namespace DataArc.Orchestration.Framework.Demo.Application.Domain.HR.ValueObjects
{
    public class EmployeeOnboardingCriteriaValueObject
    {
        public int EmployerId { get; }
        public int EmployeeId { get; }
        public bool IsArchived { get; }
        public double Rating { get; }
        public decimal AnnualSalary { get; }
        public string CurrencyCode { get; }
        public string Reason { get; }
        public DateTimeOffset EffectiveOnUtc { get; }

        public EmployeeOnboardingCriteriaValueObject(
            int employerId,
            bool isArchived,
            double rating,
            int employeeId,
            decimal annualSalary,
            string currencyCode,
            string reason,
            DateTimeOffset effectiveOnUtc
        )
        {
            EmployerId = employerId;
            IsArchived = isArchived;
            Rating = rating;
            EmployeeId = employeeId;
            AnnualSalary = annualSalary;
            CurrencyCode = currencyCode;
            Reason = reason;
            EffectiveOnUtc = effectiveOnUtc;
        }

        bool HasValidEmployer => EmployerId > 0;
        public bool MeetsOnboardingCriteria 
            => HasValidEmployer && !IsArchived && Rating > 4.0;
    }
}