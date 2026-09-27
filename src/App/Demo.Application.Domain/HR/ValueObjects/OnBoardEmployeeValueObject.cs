namespace Demo.Application.Domain.HR.ValueObjects
{
    public sealed class OnBoardEmployeeValueObject
    {
        public string? Status { get; }

        public bool IsOnboarded =>
            string.Equals(
                Status,
                "Completed",
                StringComparison.OrdinalIgnoreCase);

        public OnBoardEmployeeValueObject(string? status)
        {
            Status = status;
        }
    }
}