namespace Demo.Application.Domain.HR.ValueObjects
{
    public sealed class OnBoardEmployeeValueObject
    {
        public string? Status { get; }
        public bool UserExists { get; }
        public bool EmployeeExists { get; }
        public bool HasDepartment { get; }
        public bool PayrollRecordExists { get; }
        public bool AccessRequestExists { get; }
        public bool OnboardingTaskExists { get; }

        public bool IsOnboarded =>
            string.Equals(
                Status,
                "Completed",
                StringComparison.OrdinalIgnoreCase);

        public bool HasExistingOnboardingRecords =>
            PayrollRecordExists ||
            AccessRequestExists ||
            OnboardingTaskExists;

        public OnBoardEmployeeValueObject(
            string? status,
            bool userExists,
            bool employeeExists,
            bool hasDepartment,
            bool payrollRecordExists,
            bool accessRequestExists,
            bool onboardingTaskExists)
        {
            Status = status;
            UserExists = userExists;
            EmployeeExists = employeeExists;
            HasDepartment = hasDepartment;
            PayrollRecordExists = payrollRecordExists;
            AccessRequestExists = accessRequestExists;
            OnboardingTaskExists = onboardingTaskExists;
        }
    }
}