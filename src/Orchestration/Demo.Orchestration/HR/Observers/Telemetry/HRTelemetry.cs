using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Demo.Orchestration.HR.Telemetry
{
    internal static class HRTelemetry
    {
        public const string SourceName = "Demo.HR";
        public static readonly ActivitySource ActivitySource = new(SourceName);
        public static readonly Meter Meter = new(SourceName);
        public static readonly Counter<long> OnboardingPolicyAccepted = Meter.CreateCounter<long>("hr.employee_onboarding.policy.accepted");
        public static readonly Counter<long> OnboardingPolicyRejected = Meter.CreateCounter<long>("hr.employee_onboarding.policy.rejected");
    }
}