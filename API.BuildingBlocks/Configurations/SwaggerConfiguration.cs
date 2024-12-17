using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace API.BuildingBlocks.Configurations;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddSwaggerGenAuthentication(this IServiceCollection services)
    {
        var securityScheme = new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            In = ParameterLocation.Header,
            BearerFormat = "JWT",
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            Reference = new OpenApiReference
            {
                Id = "Bearer",
                Type = ReferenceType.SecurityScheme
            },
            Description = "Please enter your JWT with this format: ''YOUR_TOKEN''",
        };

        var securityRequirement = new OpenApiSecurityRequirement { { securityScheme, Array.Empty<string>() } };

        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);
            options.AddSecurityRequirement(securityRequirement);
        });

        return services;
    }
}