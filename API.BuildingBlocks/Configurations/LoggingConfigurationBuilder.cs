using Serilog;
using Serilog.Events;
using Serilog.Filters;

namespace API.BuildingBlocks.Configurations;

public static class LoggingConfigurationBuilder
{
    public static LoggerConfiguration Build()
    {
        var loggerConfiguration = new LoggerConfiguration();

        loggerConfiguration
            .Enrich.FromLogContext()
            .Enrich.WithEnvironmentName()
            .Enrich.WithMachineName()
            .Enrich.WithCorrelationId();

        loggerConfiguration
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error)
            .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware"));

        loggerConfiguration.WriteTo.Console();
        return loggerConfiguration;
    }
}