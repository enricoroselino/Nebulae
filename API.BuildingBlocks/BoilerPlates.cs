using System.Text.Json;
using System.Text.Json.Serialization;
using API.BuildingBlocks.Configurations;
using API.BuildingBlocks.Configurations.Interceptors;
using Asp.Versioning;
using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using Shared.Utilities.Hasher;

namespace API.BuildingBlocks;

public static class BoilerPlates
{
    public static void AddApiBuildingBlocks(this IServiceCollection services)
    {
        // one validation rule per property
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

        services.Configure<JsonOptions>(options =>
        {
            // change enums into its string representation, globally
            // same thing as [JsonConverter(typeof(JsonStringEnumConverter))]
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());

            options.SerializerOptions.PropertyNameCaseInsensitive = true;
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.SerializerOptions.WriteIndented = true;
            options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("X-Api-Version")
            );
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });

        // API documentation
        services.AddEndpointsApiExplorer();
        services.ConfigureOptions<ConfigureSwaggerOptions>();
        services.AddSwaggerGenAuthentication();

        // Authentication builder, chain here for another auth configuration
        services.AddAuthConfiguration();
        services.AddAuthorization();

        var loggerConfig = LoggingConfigurationBuilder.Build();
        var logger = Log.Logger = loggerConfig.CreateLogger();
        services.AddSerilog(logger);

        services.AddCors();
        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();

        services.AddSingleton<TimeProvider>(TimeProvider.System);
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, TimeAuditInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, SoftDeleteInterceptor>();

        services.AddScoped<IHasher>(_ => new BcryptHasher(workFactor: 14));
    }

    public static void UseApiBuildingBlocks(this WebApplication app)
    {
        // Exception handling should be at the top of the pipeline
        app.UseExceptionHandler(_ => { });

        if (!app.Environment.IsProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                var descriptions = app.DescribeApiVersions().ToList();
                descriptions.ForEach(description =>
                {
                    var url = $"/swagger/{description.GroupName}/swagger.json";
                    var name = description.GroupName.ToUpperInvariant();
                    options.SwaggerEndpoint(url, name);
                });
            });
        }

        app.UseCors(policyBuilder => policyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

        // HTTPS redirection (optional, should come after CORS and exception handling)
        app.UseHttpsRedirection();

        // Authentication/Authorization should come after exception handling
        app.UseAuthentication();
        app.UseAuthorization();

        var apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            // .HasApiVersion(new ApiVersion(2))
            .ReportApiVersions()
            .Build();

        var versionGroup = app
            .MapGroup("api/v{version:apiVersion}")
            .WithApiVersionSet(apiVersionSet);

        versionGroup.MapCarter();
    }
}