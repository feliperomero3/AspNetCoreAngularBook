using Serilog;

namespace WorldCitiesAPI.Configurations;

public static class SerilogConfiguration
{
    internal static LoggerConfiguration CreateConfiguration() =>
        new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Antiforgery", Serilog.Events.LogEventLevel.Debug)
            .MinimumLevel.Override("Microsoft.AspNetCore.DataProtection", Serilog.Events.LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", Serilog.Events.LogEventLevel.Debug)
            .MinimumLevel.Override("Microsoft.AspNetCore.Authorization", Serilog.Events.LogEventLevel.Debug)
            .MinimumLevel.Override("Microsoft.AspNetCore.Identity", Serilog.Events.LogEventLevel.Debug)
            .WriteTo.Debug(outputTemplate: "{SourceContext} [{Level}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.Console(outputTemplate: "{SourceContext} [{Level}] {Message:lj}{NewLine}{Exception}");
}
