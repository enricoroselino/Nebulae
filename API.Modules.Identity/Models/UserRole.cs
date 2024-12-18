namespace API.Modules.Identity.Models;

public class UserRole
{
    public UserId UserId { get; private set; }
    public RoleId RoleId { get; private set; }

    private UserRole() { } // Required by EF Core
    
    // Navigation
    public virtual User User { get; private set; }
    public virtual Role Role { get; private set; }

    public UserRole(UserId userId, RoleId roleId)
    {
        UserId = userId;
        RoleId = roleId;
    }
}