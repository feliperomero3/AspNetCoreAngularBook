using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace WorldCitiesAPI.Tests.Extensions;

public class TestAuthenticationHandler : IAuthenticationHandler
{
    public Task<AuthenticateResult> AuthenticateAsync()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "Test user"),
            new Claim(ClaimTypes.Email, "test@example.com"),
            new Claim(ClaimTypes.Role, "User")
        };

        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Test");

        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }

    public Task ChallengeAsync(AuthenticationProperties? properties)
    {
        return Task.CompletedTask;
    }

    public Task ForbidAsync(AuthenticationProperties? properties)
    {
        return Task.CompletedTask;
    }

    public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
    {
        return Task.CompletedTask;
    }
}
