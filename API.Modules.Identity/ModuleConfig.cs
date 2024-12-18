using API.Shared.Extensions;

namespace API.Modules.Identity;

public static class ModuleConfig
{
    public const string IamGroup = "iam";
    
    public static void AddIdentityModule(this IServiceCollection services)
    {
        services.AddAssemblyScan(typeof(ModuleConfig).Assembly);
        services.AddDbContext<AppIdentityDbContext>();
    }
}