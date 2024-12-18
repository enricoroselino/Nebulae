using System.Reflection;
using API.Shared.Behaviors;
using Carter;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shared.Helpers;

namespace API.Shared.Extensions;

public static class AssemblyStackExtension
{
    public static void AddAssemblyScan(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddCarter(configurator: configurator =>
        {
            var modules = AssembliesHelper.GetInterfaceTypes<ICarterModule>(assemblies);
            configurator.WithModules(modules);
        });

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(assemblies);
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        services.AddValidatorsFromAssemblies(assemblies);
    }
}