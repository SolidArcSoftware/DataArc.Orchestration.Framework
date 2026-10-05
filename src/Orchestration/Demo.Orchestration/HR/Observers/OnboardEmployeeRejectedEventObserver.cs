using Microsoft.Extensions.Logging;
using System.Diagnostics;

using DataArc.Observer;

using Demo.Application.Domain.HR.Events;
using Demo.Orchestration.HR.Telemetry;

namespace Demo.Orchestration.HR.Observers
{
    internal sealed class OnboardEmployeeRejectedEventObserver
        : IEventObserver<OnboardEmployeeRejectedEvent>
    {
        private readonly ILogger<OnboardEmployeeRejectedEventObserver> _logger;

        public OnboardEmployeeRejectedEventObserver(
            ILogger<OnboardEmployeeRejectedEventObserver> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(OnboardEmployeeRejectedEvent evt)
        {
            using var activity =
                HRTelemetry.ActivitySource.StartActivity(
                    "HR Employee Onboarding Policy Rejected",
                    ActivityKind.Internal);

            activity?.SetTag(
                "hr.employee_onboarding.policy.result",
                "rejected");

            activity?.SetTag(
                "hr.employee_onboarding.policy.reason",
                evt.Reason);

            HRTelemetry.OnboardingPolicyRejected.Add(
                1,
                new KeyValuePair<string, object?>(
                    "result",
                    "rejected"));

            _logger.LogWarning(
                "Employee onboarding policy rejected the request. Reason: {Reason}",
                evt.Reason);

            return Task.CompletedTask;
        }
    }
}