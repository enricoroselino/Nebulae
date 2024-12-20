using Ardalis.Specification;

namespace API.Modules.Identity.Persistence.Specifications;

public sealed class GetUserSpecification : Specification<User>
{
    public GetUserSpecification(
        UserId? userId = null,
        string? username = null,
        string? email = null)
    {
        if (userId is null && username is null && email is null)
            throw new Exception("UserId or Username or Email cannot be empty");

        if (userId != null) Query.Where(u => u.Id == userId);
        if (!string.IsNullOrEmpty(username)) Query.Where(u => u.Username == username);
        if (!string.IsNullOrEmpty(email)) Query.Where(u => EF.Functions.Like(u.Email, email));
    }
}