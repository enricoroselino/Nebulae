using System.Reflection;
using API.Shared.Behaviors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace API.Shared.Extensions;

public static class MediatorExtension
{
    public static IServiceCollection AddMediatorFromAssemblies(this IServiceCollection services,
        params Assembly[] assemblies)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(assemblies);
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        services.AddValidatorsFromAssemblies(assemblies);
        return services;
    }
}