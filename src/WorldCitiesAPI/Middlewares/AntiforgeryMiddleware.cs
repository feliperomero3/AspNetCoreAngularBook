using Microsoft.AspNetCore.Antiforgery;

namespace WorldCitiesAPI.Middlewares;

public class AntiforgeryMiddleware : IMiddleware
{
    private readonly IAntiforgery _antiforgery;
    private readonly IEnumerable<PathString> _excludedPaths = new[]
    {
        new PathString("/api/account/login"),
        new PathString("/api/account/logout")
    };

    public AntiforgeryMiddleware(IAntiforgery antiforgery)
    {
        _antiforgery = antiforgery ?? throw new ArgumentNullException(nameof(antiforgery));
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var request = context.Request;

        if (_excludedPaths.Contains(request.Path))
        {
            await next(context);

            return;
        }

        if (!await _antiforgery.IsRequestValidAsync(context))
        {
            await _antiforgery.ValidateRequestAsync(context);
        }
        await next(context);
    }
}
