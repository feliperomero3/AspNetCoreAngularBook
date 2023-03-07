using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WorldCitiesAPI.Configurations;
using WorldCitiesAPI.Data;
using WorldCitiesAPI.Entities;
using WorldCitiesAPI.Middlewares;

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
        builder.Services.AddHttpContextAccessor();
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

        builder.Services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");
        builder.Services.AddScoped<AntiforgeryMiddleware>();

        builder.Services.AddHealthChecks();

        builder.Services.AddCors(options =>
            options.AddPolicy(name: "AngularPolicy",
            policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.WithOrigins(builder.Configuration["AllowedCors"]!);
            }));

        builder.Host.UseSerilog();

        var app = builder.Build();

        app.UseSerilogRequestLogging(options => options.IncludeQueryInRequestPath = true);

        using (var scope = app.Services.CreateScope())
        {
            scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>().Initialize();
        }

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();
        app.UseCors("AngularPolicy");

        app.MapHealthChecks("/api/heartbeat").AllowAnonymous();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseAntiforgery();

        app.MapControllers();

        app.Run();
    }
}
