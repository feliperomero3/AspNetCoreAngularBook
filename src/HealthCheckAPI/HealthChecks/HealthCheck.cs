using System.Diagnostics;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthCheckAPI.HealthChecks;

public class HealthCheck : IHealthCheck
{
    private readonly HttpClient _client;
    private readonly string _host;
    private readonly long _healthyRoundtripTime;

    public HealthCheck(string host, long healthyRoundtripTime)
    {
        _host = host ?? throw new ArgumentNullException(nameof(host));
        _healthyRoundtripTime = healthyRoundtripTime;
        _client = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(1)
        };
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var stopWatch = Stopwatch.StartNew();

            var response = await _client.GetAsync(_host, cancellationToken);

            stopWatch.Stop();

            response.EnsureSuccessStatusCode();

            var message = $"Request sent to {_host}.";

            return stopWatch.ElapsedMilliseconds > _healthyRoundtripTime
                ? HealthCheckResult.Degraded(message)
                : HealthCheckResult.Healthy(message);
        }
        catch (HttpRequestException exception)
        {
            var errorMessage = $"Request sent to {_host} failed: {exception.Message}";

            return HealthCheckResult.Unhealthy(errorMessage);
        }
    }
}
