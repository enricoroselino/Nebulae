using API.Modules.Identity.Models.Interfaces;

namespace API.Modules.Identity.Models;

public record UserClaimId(int Value);

public class UserClaim : Entity<UserClaimId>, IClaim
{
    private UserClaim()
    {
    }

    public UserId UserId { get; private set; } = null!;
    public string ClaimType { get; private set; } = string.Empty;
    public string ClaimValue { get; private set; } = string.Empty;

    // navigation
    public virtual User User { get; private set; } = null!;

    public static UserClaim Create(UserId userId, string claimType, string claimValue)
    {
        return new UserClaim()
        {
            UserId = userId,
            ClaimType = claimType,
            ClaimValue = claimValue
        };
    }
}