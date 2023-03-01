using Microsoft.AspNetCore.TestHost;

namespace WorldCitiesAPI.Tests.Extensions;

public static class WebHostBuilderExtensions
{
    public static IWebHostBuilder WithAuthenticatedUser(this IWebHostBuilder builder)
    {
        return builder.ConfigureTestServices(services =>
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Test";
                options.AddScheme<TestAuthenticationHandler>("Test", string.Empty);
            });
        });
    }
}
