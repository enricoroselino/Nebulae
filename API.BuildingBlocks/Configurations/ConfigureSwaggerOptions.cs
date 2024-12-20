using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.BuildingBlocks.Configurations;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

    public void Configure(SwaggerGenOptions options)
    {
        var descriptions = _provider.ApiVersionDescriptions.ToList();
        descriptions.ForEach(description =>
        {
            var apiVersionInfo = CreateInfoForApiVersion(description);
            options.SwaggerDoc(description.GroupName, apiVersionInfo);
        });
    }

    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var solutionFullPath = Directory.GetParent(Directory.GetCurrentDirectory())!.FullName;
        var solutionName = solutionFullPath.Split('\\')[^1];

        var info = new OpenApiInfo()
        {
            Title = $"{solutionName} - {description.GroupName.ToUpperInvariant()}",
            Version = description.ApiVersion.ToString(),
            Description = $"The {solutionName} API provides secure, efficient access to core functionalities. This API ensures compliance with industry standards and is built on clean code principles, leveraging Domain-Driven Design (DDD) and CQRS for scalability and maintainability.",
            License = new OpenApiLicense { Name = "WTFPL", Url = new Uri("https://en.wikipedia.org/wiki/WTFPL") }
        };

        if (description.IsDeprecated)
        {
            info.Description += " This API version has been deprecated.";
        }

        return info;
    }
}