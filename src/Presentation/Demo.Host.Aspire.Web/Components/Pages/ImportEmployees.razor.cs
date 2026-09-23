using System.Diagnostics;

using Microsoft.AspNetCore.Components;

namespace Demo.Host.Aspire.Web.Components.Pages;

public partial class ImportEmployees
{
    [Inject]
    public IHttpClientFactory HttpClientFactory { get; set; } = null!;

    protected bool IsImporting { get; set; }

    protected string? SuccessMessage { get; set; }

    protected string? ErrorMessage { get; set; }

    protected async Task ImportEmployeesAsync()
    {
        IsImporting = true;
        SuccessMessage = null;
        ErrorMessage = null;

        var stopwatch = Stopwatch.StartNew();

        try
        {
            var client = HttpClientFactory.CreateClient("DataArcApi");

            using var response = await client.PostAsync(
                "/api/hr/imports",
                content: null);

            stopwatch.Stop();

            if (!response.IsSuccessStatusCode)
            {
                ErrorMessage =
                    $"Employee import failed after {stopwatch.ElapsedMilliseconds:N0} milliseconds.";

                return;
            }

            var result =
                await response.Content.ReadFromJsonAsync<ImportEmployeesResponse>();

            if (result == null)
            {
                ErrorMessage =
                    "Employee import completed but no response was returned.";

                return;
            }

            if (result.Errors is { Count: > 0 })
            {
                ErrorMessage =
                    $"Employee import completed with {result.Errors.Count:N0} error(s).";

                return;
            }

            SuccessMessage =
                $"{result.TotalRecordsProcessed:N0} employees imported in " +
                $"{stopwatch.ElapsedMilliseconds:N0} milliseconds.";
        }
        catch (Exception exception)
        {
            stopwatch.Stop();

            ErrorMessage =
                $"Employee import failed after " +
                $"{stopwatch.ElapsedMilliseconds:N0} milliseconds. " +
                $"{exception.Message}";
        }
        finally
        {
            IsImporting = false;
        }
    }

    private sealed class ImportEmployeesResponse
    {
        public int TotalRecordsProcessed { get; set; }

        public List<string>? Errors { get; set; }
    }
}