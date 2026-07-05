namespace DataArc.Orchestration.Framework.Demo.Application.Domain.HR.ValueObjects
{
    public sealed class OnBoardEmployeeValueObject
    {
        public string? Status { get; }
        public int? PayrollRecordId { get; }
        public int? AccessRequestId { get; }
        public int? OnboardingTaskId { get; }
        public string? PayrollCurrencyCode { get; }
        public bool? PayrollIsActive { get; }
        public string? AccessRequestStatus { get; }
        public string? OnboardingTaskStatus { get; }

        public OnBoardEmployeeValueObject(
            string? status,
            int? payrollRecordId,
            int? accessRequestId,
            int? onboardingTaskId,
            string? payrollCurrencyCode,
            bool? payrollIsActive,
            string? accessRequestStatus,
            string? onboardingTaskStatus)
        {
            Status = status;
            PayrollRecordId = payrollRecordId;
            AccessRequestId = accessRequestId;
            OnboardingTaskId = onboardingTaskId;
            PayrollCurrencyCode = payrollCurrencyCode;
            PayrollIsActive = payrollIsActive;
            AccessRequestStatus = accessRequestStatus;
            OnboardingTaskStatus = onboardingTaskStatus;
        }

        public bool IsActive =>
            string.Equals(Status, "Active", StringComparison.OrdinalIgnoreCase);

        public bool HasPayrollRecord =>
            PayrollRecordId.HasValue;

        public bool HasAccessRequest =>
            AccessRequestId.HasValue;

        public bool HasOnboardingTask =>
            OnboardingTaskId.HasValue;

        public bool HasActivePayroll =>
            PayrollIsActive == true;

        public bool HasRequestedAccess =>
            string.Equals(AccessRequestStatus, "Requested", StringComparison.OrdinalIgnoreCase);

        public bool HasCreatedOnboardingTask =>
            string.Equals(OnboardingTaskStatus, "Created", StringComparison.OrdinalIgnoreCase);

        public bool HasExistingOnboardingWorkflow =>
            HasPayrollRecord ||
            HasAccessRequest ||
            HasOnboardingTask;

        public bool HasCompletedOnboardingWorkflow =>
            HasActivePayroll &&
            HasRequestedAccess &&
            HasCreatedOnboardingTask;
    }
}