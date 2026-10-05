using Microsoft.Extensions.Logging;
using System.Diagnostics;

using DataArc.Observer;

using Demo.Application.Domain.HR.Events;
using Demo.Orchestration.HR.Telemetry;

namespace Demo.Orchestration.HR.Observers
{
    internal sealed class OnboardEmployeeAcceptedEventObserver
        : IEventObserver<OnboardEmployeeAcceptedEvent>
    {
        private readonly ILogger<OnboardEmployeeAcceptedEventObserver> _logger;

        public OnboardEmployeeAcceptedEventObserver(
            ILogger<OnboardEmployeeAcceptedEventObserver> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(OnboardEmployeeAcceptedEvent evt)
        {
            using var activity =
                HRTelemetry.ActivitySource.StartActivity(
                    "HR Employee Onboarding Policy Accepted",
                    ActivityKind.Internal);

            activity?.SetTag(
                "hr.employee_onboarding.policy.result",
                "accepted");

            activity?.SetTag(
                "hr.employee_onboarding.policy.reason",
                evt.Reason);

            HRTelemetry.OnboardingPolicyAccepted.Add(
                1,
                new KeyValuePair<string, object?>(
                    "result",
                    "accepted"));

            _logger.LogInformation(
                "Employee onboarding policy accepted the request. Reason: {Reason}",
                evt.Reason);

            return Task.CompletedTask;
        }
    }
}