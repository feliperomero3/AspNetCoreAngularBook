using Microsoft.Net.Http.Headers;

namespace WorldCitiesAPI.Tests.Controllers;

public class XsrfControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public XsrfControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _factory.ClientOptions.BaseAddress = new Uri("https://localhost:7063");
    }

    [Fact]
    public async Task Get_xsrf_token()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/xsrf-token");

        response.EnsureSuccessStatusCode();

        var xsrfToken = response.Headers.FirstOrDefault(h => h.Key == HeaderNames.SetCookie).Value;

        Assert.True(xsrfToken.Any());
    }
}
