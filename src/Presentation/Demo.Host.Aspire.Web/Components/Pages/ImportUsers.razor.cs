using System.Diagnostics;

using Microsoft.AspNetCore.Components;

namespace Demo.Host.Aspire.Web.Components.Pages;

public partial class ImportUsers
{
    [Inject]
    public IHttpClientFactory HttpClientFactory { get; set; } = null!;

    protected bool IsImporting { get; set; }

    protected string? SuccessMessage { get; set; }

    protected string? ErrorMessage { get; set; }

    protected async Task ImportUsersAsync()
    {
        IsImporting = true;
        SuccessMessage = null;
        ErrorMessage = null;

        var stopwatch = Stopwatch.StartNew();

        try
        {
            var client = HttpClientFactory.CreateClient("DataArcApi");

            using var response = await client.PostAsync(
                "/api/auth/imports",
                content: null);

            stopwatch.Stop();

            if (!response.IsSuccessStatusCode)
            {
                ErrorMessage =
                    $"User import failed after {stopwatch.ElapsedMilliseconds:N0} milliseconds.";

                return;
            }

            var result =
                await response.Content.ReadFromJsonAsync<ImportEmployeesResponse>();

            if (result == null)
            {
                ErrorMessage =
                    "User import completed but no response was returned.";

                return;
            }

            if (result.Errors is { Count: > 0 })
            {
                ErrorMessage =
                    $"User import completed with {result.Errors.Count:N0} error(s).";

                return;
            }

            SuccessMessage =
                $"{result.TotalRecordsProcessed:N0} users imported in " +
                $"{stopwatch.ElapsedMilliseconds:N0} milliseconds.";
        }
        catch (Exception exception)
        {
            stopwatch.Stop();

            ErrorMessage =
                $"User import failed after " +
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