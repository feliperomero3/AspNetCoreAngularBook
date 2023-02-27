namespace WorldCitiesAPI.Tests.Controllers;

public class CitiesControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public CitiesControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _factory.ClientOptions.BaseAddress = new Uri("https://localhost:7063");
    }

    [Fact]
    public async Task Get_cities()
    {
        // Arrange
        var client = _factory.CreateClient();

        var result = await client.PostAsJsonAsync("/api/account/login",
            new LoginRequest { Email = "alice@example.com", Password = "password" });

        result.EnsureSuccessStatusCode();

        // Act
        var response = await client.GetAsync("/api/cities");

        // Assert
        response.EnsureSuccessStatusCode();

        Assert.Equal("application/json; charset=utf-8",
            response.Content.Headers.ContentType?.ToString());
    }
}
