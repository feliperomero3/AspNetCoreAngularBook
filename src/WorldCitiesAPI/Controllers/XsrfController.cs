using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WorldCitiesAPI.Controllers;

[AllowAnonymous]
[Route("api/xsrf-token")]
[ApiController]
public class XsrfController : ControllerBase
{
    private readonly IAntiforgery _antiforgery;
    private readonly IHttpContextAccessor _contextAccessor;

    public XsrfController(IAntiforgery antiforgery, IHttpContextAccessor contextAccessor)
    {
        _antiforgery = antiforgery ?? throw new ArgumentNullException(nameof(antiforgery));
        _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
    }

    [HttpGet]
    public ActionResult GetXsrfToken()
    {
        var context = _contextAccessor.HttpContext!;
        var tokenSet = _antiforgery.GetAndStoreTokens(context);

        context.Response.Cookies.Append("XSRF-TOKEN", tokenSet.RequestToken!, new CookieOptions { HttpOnly = false });

        return NoContent();
    }
}
