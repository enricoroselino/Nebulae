namespace API.Modules.Identity.Dtos;

public class UserDto
{
    public Guid Id { get; private set; }
    public int? CompatId { get; private set; }
    public string FullName { get; private set; } = string.Empty;
    public string Username { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;

    public static UserDto Create(User user)
    {
        return new UserDto()
        {
            Id = user.Id.Value,
            CompatId = user.CompatId,
            FullName = user.Fullname,
            Username = user.Username,
            Email = user.Email,
        };
    }
}