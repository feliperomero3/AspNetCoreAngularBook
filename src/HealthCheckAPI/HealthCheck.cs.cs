using System.Diagnostics;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthCheckAPI;

public class HealthCheck : IHealthCheck
{
    private readonly HttpClient _client;
    private const string _host = "https://10.0.0.0";
    private const long _healthyRoundtripTime = 300L;

    public HealthCheck(HttpClient client)
    {
        _client = client;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var stopWatch = Stopwatch.StartNew();

            var response = await _client.GetAsync(_host, cancellationToken);

            stopWatch.Stop();

            response.EnsureSuccessStatusCode();

            return (stopWatch.ElapsedMilliseconds > _healthyRoundtripTime)
                ? HealthCheckResult.Degraded()
                : HealthCheckResult.Healthy();
        }
        catch (HttpRequestException exception)
        {
            return HealthCheckResult.Unhealthy("Unhealthy", exception);
        }
    }
}
