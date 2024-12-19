using API.Modules.Identity;

namespace API.Server;

public static class ModuleDiscovery
{
    public static void AddModules(this IServiceCollection services)
    {
        services.AddIdentityModule();
    }

    public static void UseModules(this WebApplication app)
    {
        app.UseIdentityModule();
    }
}