namespace API.Modules.Identity.Dtos;

public class RoleDto
{
    private RoleDto()
    {
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;

    public static RoleDto Create(Role role)
    {
        return new RoleDto()
        {
            Id = role.Id.Value,
            Name = role.Name
        };
    }
}