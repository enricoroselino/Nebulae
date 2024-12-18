namespace API.Modules.Identity.Models;

public record RoleId(Guid Value);

public class Role : Entity<RoleId>
{
    public string Name { get; private set; }

    // Required by EF Core
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