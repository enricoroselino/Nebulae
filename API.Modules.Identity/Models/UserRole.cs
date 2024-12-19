namespace API.Modules.Identity.Models;

public class UserRole
{
    private UserRole()
    {
    }
    
    public UserId UserId { get; private set; }
    public RoleId RoleId { get; private set; }

    // Navigation
    public virtual User User { get; private set; }
    public virtual Role Role { get; private set; }

    public static UserRole Create(UserId userId, RoleId roleId)
    {
        return new UserRole()
        {
            UserId = userId,
            RoleId = roleId
        };
    }
}