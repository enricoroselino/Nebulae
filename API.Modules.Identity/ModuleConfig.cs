using API.Modules.Identity.Persistence;
using API.Shared.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace API.Modules.Identity;

public static class ModuleConfig
{
    public static void AddIdentityModule(this IServiceCollection services)
    {
        services.AddAssemblyScan(typeof(ModuleConfig).Assembly);
        services.AddDbContext<AppIdentityDbContext>();
    }
}