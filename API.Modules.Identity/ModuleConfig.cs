using API.Shared.Extensions;

namespace API.Modules.Identity;

public static class ModuleConfig
{
    internal const string IamGroup = "iam";
    internal const string AuthGroup = "auth";

    public static void AddIdentityModule(this IServiceCollection services)
    {
        services.AddAssemblyScan(typeof(ModuleConfig).Assembly);
        services.AddDbContext<AppIdentityDbContext>((provider, builder) =>
        {
            var interceptors = provider.GetServices<ISaveChangesInterceptor>();
            builder.AddInterceptors(interceptors);
        });

        services.AddScoped<IIdentityRepository, IdentityRepository>();
    }

    public static void UseIdentityModule(this WebApplication app)
    {
        app.MigrateDatabase<AppIdentityDbContext>().GetAwaiter().GetResult();
    }
}