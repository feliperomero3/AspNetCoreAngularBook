using Microsoft.Net.Http.Headers;

namespace WorldCitiesAPI.Tests.Controllers;

public class AccountControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public AccountControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _factory.ClientOptions.BaseAddress = new Uri("https://localhost:7063/api/account/");
    }

    [Fact]
    public async Task User_can_signin()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync("login",
            new LoginRequest { Email = "alice@example.com", Password = "password" });

        response.EnsureSuccessStatusCode();

        var authenticationCookie = response.Headers.FirstOrDefault(h => h.Key == HeaderNames.SetCookie).Value.FirstOrDefault();

        Assert.StartsWith(".AspNetCore.Identity.Application", authenticationCookie);
    }

    [Fact]
    public async Task User_can_signout()
    {
        var client = _factory.CreateClient();

        var loginResponse = await client.PostAsJsonAsync("login",
            new LoginRequest { Email = "alice@example.com", Password = "password" });

        loginResponse.EnsureSuccessStatusCode();

        var logoutResponse = await client.PostAsync("logout", new StringContent(string.Empty));

        logoutResponse.EnsureSuccessStatusCode();
    }
}
