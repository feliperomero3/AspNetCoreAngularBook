using Serilog;

namespace WorldCitiesAPI.Configurations;

public static class SerilogConfiguration
{
    internal static LoggerConfiguration CreateConfiguration() =>
        new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", Serilog.Events.LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.AspNetCore.Identity", Serilog.Events.LogEventLevel.Debug)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", Serilog.Events.LogEventLevel.Information)
            .WriteTo.Debug(outputTemplate: "{SourceContext} [{Level}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.Console(outputTemplate: "{SourceContext} [{Level}] {Message:lj}{NewLine}{Exception}");
}
