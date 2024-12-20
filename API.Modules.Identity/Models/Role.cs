namespace API.Modules.Identity.Models;

public record RoleId(Guid Value);

public class Role : Entity<RoleId>
{
    public string Name { get; private set; } = string.Empty;

    // navigation
    public virtual ICollection<RoleClaim> RoleClaims { get; private set; } = null!;
    public virtual ICollection<UserRole> UserRoles { get; private set; } = null!;

    private Role()
    {
    }

    public static Role Create(string name)
    {
        return new Role()
        {
            Id = new RoleId(Guid.CreateVersion7()),
            Name = name
        };
    }
}