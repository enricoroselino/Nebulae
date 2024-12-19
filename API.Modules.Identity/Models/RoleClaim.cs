using API.Modules.Identity.Models.Interfaces;

namespace API.Modules.Identity.Models;

public record RoleClaimId(int Value);

public class RoleClaim : Entity<RoleClaimId>, IClaim
{
    private RoleClaim()
    {
    }

    public RoleId RoleId { get; private set; } = null!;
    public string ClaimType { get; private set; } = string.Empty;
    public string ClaimValue { get; private set; } = string.Empty;

    // navigation
    public virtual Role Role { get; private set; } = null!;

    public static RoleClaim Create(RoleId roleId, string claimType, string claimValue)
    {
        return new RoleClaim()
        {
            RoleId = roleId,
            ClaimType = claimType,
            ClaimValue = claimValue
        };
    }
}