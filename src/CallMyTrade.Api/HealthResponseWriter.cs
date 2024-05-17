using System.Reflection;
using System.Text.Json;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CallMyTrade;

internal static class HealthResponseWriter
{
    private static readonly string Version =
        Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? string.Empty;

    public static Task WriteResponseAsync(HttpContext context, HealthReport healthReport)
    {
        var response = new
        {
            Status = healthReport.Status.ToString(),
            Reports = healthReport.Entries.Select(kvp => new
            {
                Name = kvp.Key,
                Status = kvp.Value.Status.ToString(),
                kvp.Value.Description,
                kvp.Value.Exception,
                kvp.Value.Duration,
                kvp.Value.Tags
            }),
            Version
        };
        context.Response.ContentType = "application/json; charset=utf-8";
        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}