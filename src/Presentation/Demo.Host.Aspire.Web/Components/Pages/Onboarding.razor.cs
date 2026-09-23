using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;

using Microsoft.AspNetCore.Components;

namespace Demo.Host.Aspire.Web.Components.Pages;

public partial class Onboarding
{
    [Inject]
    public IHttpClientFactory HttpClientFactory { get; set; } = null!;

    protected int EmployeeId { get; set; } = 1;

    protected decimal AnnualSalary { get; set; } = 85_000;

    protected string CurrencyCode { get; set; } = "USD";

    protected string Reason { get; set; } = "Demo employee onboarding";

    protected bool IsOnboarding { get; set; }

    protected string? SuccessMessage { get; set; }

    protected string? ErrorMessage { get; set; }

    protected async Task OnboardEmployeeAsync()
    {
        IsOnboarding = true;
        SuccessMessage = null;
        ErrorMessage = null;

        var stopwatch = Stopwatch.StartNew();

        try
        {
            var client = HttpClientFactory.CreateClient("DataArcApi");

            var request = new OnboardEmployeeRequest
            {
                EmployeeId = EmployeeId,
                AnnualSalary = AnnualSalary,
                CurrencyCode = CurrencyCode,
                Reason = Reason
            };

            using var response = await client.PostAsJsonAsync(
                "/api/hr/onboarding",
                request);

            stopwatch.Stop();

            if (!response.IsSuccessStatusCode)
            {
                ErrorMessage = await GetFailureMessageAsync(
                    response,
                    stopwatch.ElapsedMilliseconds);

                return;
            }

            var result =
                await response.Content.ReadFromJsonAsync<OnboardEmployeeResponse>();

            if (result == null)
            {
                ErrorMessage =
                    "Employee onboarding completed but no response was returned.";

                return;
            }

            SuccessMessage =
                $"Employee {result.EmployeeId:N0} " +
                $"{result.Name} {result.Surname} was onboarded in " +
                $"{stopwatch.ElapsedMilliseconds:N0} milliseconds. " +
                $"Payroll record {result.PayrollRecordId:N0} was created.";
        }
        catch (Exception exception)
        {
            stopwatch.Stop();

            ErrorMessage =
                $"Employee onboarding failed after " +
                $"{stopwatch.ElapsedMilliseconds:N0} milliseconds. " +
                $"{exception.Message}";
        }
        finally
        {
            IsOnboarding = false;
        }
    }

    private static async Task<string> GetFailureMessageAsync(
        HttpResponseMessage response,
        long elapsedMilliseconds)
    {
        var content = await response.Content.ReadAsStringAsync();

        if (!string.IsNullOrWhiteSpace(content))
        {
            try
            {
                using var document = JsonDocument.Parse(content);

                if (document.RootElement.TryGetProperty(
                        "detail",
                        out var detailElement))
                {
                    var detail = detailElement.GetString();

                    if (!string.IsNullOrWhiteSpace(detail))
                    {
                        return
                            $"Employee onboarding was rejected after " +
                            $"{elapsedMilliseconds:N0} milliseconds. {detail}";
                    }
                }
            }
            catch (JsonException)
            {
                // Fall through to the generic response below.
            }
        }

        return
            $"Employee onboarding failed after " +
            $"{elapsedMilliseconds:N0} milliseconds.";
    }

    private sealed class OnboardEmployeeRequest
    {
        public int EmployeeId { get; set; }

        public decimal AnnualSalary { get; set; }

        public string CurrencyCode { get; set; } = string.Empty;

        public string Reason { get; set; } = string.Empty;
    }

    private sealed class OnboardEmployeeResponse
    {
        public bool IsSuccess { get; set; }

        public string? FailureReason { get; set; }

        public int EmployeeId { get; set; }

        public string? Name { get; set; }

        public string? Surname { get; set; }

        public string? Status { get; set; }

        public double Rating { get; set; }

        public decimal Salary { get; set; }

        public int PayrollRecordId { get; set; }
    }
}