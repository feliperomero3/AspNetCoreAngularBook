using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WorldCitiesAPI.Configurations;
using WorldCitiesAPI.Data;
using WorldCitiesAPI.Entities;

namespace WorldCitiesAPI;

public class Program
{
    private static void Main(string[] args)
    {
        Log.Logger = SerilogConfiguration
            .CreateConfiguration()
            .CreateLogger();

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            options.EnableSensitiveDataLogging();
        });

        builder.Services.AddScoped<ApplicationDbContextInitializer>();

        builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireDigit = false;
        }).AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/api/account/login";
            options.AccessDeniedPath = "/api/account/denied";
            options.EventsType = typeof(CustomCookieAuthenticationEvents);
            options.Cookie.HttpOnly = false;
        }).AddScoped<CustomCookieAuthenticationEvents>();

        builder.Services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            options.AddPolicy("UserPolicy", policyBuilder => policyBuilder.RequireRole("Administrator", "User"));
            options.AddPolicy("AdministratorPolicy", policyBuilder => policyBuilder.RequireRole("Administrator"));
        });

        builder.Host.UseSerilog();

        var app = builder.Build();

        app.UseSerilogRequestLogging();

        using (var scope = app.Services.CreateScope())
        {
            scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>().Initialize();
        }

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
