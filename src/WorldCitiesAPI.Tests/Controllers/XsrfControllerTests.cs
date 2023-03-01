using System.Net.Http.Headers;
using WorldCitiesAPI.Tests.Extensions;

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
        var client = _factory.WithAuthenticatedUser().CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

        var response = await client.GetAsync("/api/xsrf-token");

        response.EnsureSuccessStatusCode();

        var xsrfToken = response.Headers.FirstOrDefault(h => h.Key == "Set-Cookie").Value;

        Assert.NotNull(xsrfToken);
    }
}
