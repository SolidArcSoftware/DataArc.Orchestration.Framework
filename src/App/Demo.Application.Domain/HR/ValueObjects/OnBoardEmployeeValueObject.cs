namespace Demo.Application.Domain.HR.ValueObjects
{
    public sealed class OnBoardEmployeeValueObject
    {
        public string? Status { get; }

        public OnBoardEmployeeValueObject(string? status)
        {
            Status = status;
        }

        public bool IsOnboarded =>
            string.Equals(Status, "Active", StringComparison.OrdinalIgnoreCase);
    }
}