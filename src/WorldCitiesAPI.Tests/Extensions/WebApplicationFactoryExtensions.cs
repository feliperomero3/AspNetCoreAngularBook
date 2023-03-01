namespace WorldCitiesAPI.Tests.Extensions;

public static class WebApplicationFactoryExtensions
{
    public static WebApplicationFactory<Program> WithAuthenticatedUser(this WebApplicationFactory<Program> factory)
    {
        return factory.WithWebHostBuilder(builder =>
        {
            builder.WithAuthenticatedUser();
        });
    }
}
