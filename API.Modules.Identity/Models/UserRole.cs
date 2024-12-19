namespace API.Modules.Identity.Models;

public class UserRole
{
    private UserRole()
    {
    }

    public UserId UserId { get; private set; } = null!;
    public RoleId RoleId { get; private set; } = null!;

    // Navigation
    public virtual User User { get; private set; } = null!;
    public virtual Role Role { get; private set; } = null!;

    public static UserRole Create(UserId userId, RoleId roleId)
    {
        return new UserRole()
        {
            UserId = userId,
            RoleId = roleId
        };
    }
}