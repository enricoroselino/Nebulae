using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace API.BuildingBlocks.Configurations;

public static class AuthenticationConfiguration
{
    public static AuthenticationBuilder AddAuthConfiguration(this IServiceCollection services)
    {
        services
            .AddOptions<TokenProviderOptions>()
            .BindConfiguration(TokenProviderOptions.Section)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        using var serviceProvider = services.BuildServiceProvider();
        var tokenOptions = serviceProvider.GetRequiredService<IOptions<TokenProviderOptions>>();

        var key = Encoding.UTF8.GetBytes(tokenOptions.Value.Key);
        var symmetricKey = new SymmetricSecurityKey(key);
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = tokenOptions.Value.ValidIssuer,
            ValidAudiences = tokenOptions.Value.ValidAudiences,
            IssuerSigningKey = symmetricKey,
            ClockSkew = TimeSpan.Zero,
        };

        return services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = false;
                options.TokenValidationParameters = tokenValidationParameters;
            });
    }
}