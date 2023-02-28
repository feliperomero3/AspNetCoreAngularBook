namespace WorldCitiesAPI.Middlewares;

/// <summary>
/// Extension methods to add antiforgery token capabilities to an HTTP application pipeline.
/// </summary>
public static class AntiforgeryAppBuilderExtensions
{
    /// <summary>
    /// Adds the <see cref="AntiforgeryMiddleware"/> to the specified <see cref="IApplicationBuilder"/>, which enables antiforgery token capabilities.
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseAntiforgery(this IApplicationBuilder app)
    {
        if (app is null)
        {
            throw new ArgumentNullException(nameof(app));
        }

        return app.UseMiddleware<AntiforgeryMiddleware>();
    }
}
