using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace HealthCheckAPI.HealthChecks;

public class CustomHealthCheckOptions : HealthCheckOptions
{
    public CustomHealthCheckOptions() : base()
    {
        var jsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        ResponseWriter = async (context, healthReport) =>
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = StatusCodes.Status200OK;

            var result = JsonSerializer.Serialize(new
            {
                Checks = healthReport.Entries.Select(entry => new
                {
                    Name = entry.Key,
                    ResponseTime = entry.Value.Duration.TotalMilliseconds,
                    Status = entry.Value.Status.ToString(),
                    entry.Value.Description
                }),
                TotalStatus = healthReport.Status.ToString(),
                TotalResponseTime = healthReport.TotalDuration.TotalMilliseconds
            }, jsonSerializerOptions);

            await context.Response.WriteAsync(result);
        };
    }
}
