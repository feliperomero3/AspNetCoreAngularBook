namespace WorldCitiesAPI.Tests.Controllers;

public class CitiesControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public CitiesControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _factory.ClientOptions.BaseAddress = new Uri("https://localhost:7063/api/");
    }

    [Fact]
    public async Task Get_cities()
    {
        // Arrange
        var client = _factory.WithAuthenticatedUser().CreateClient();

        // Act
        var response = await client.GetAsync("cities");

        // Assert
        response.EnsureSuccessStatusCode();

        Assert.Equal("application/json; charset=utf-8",
            response.Content.Headers.ContentType?.ToString());
    }
}
