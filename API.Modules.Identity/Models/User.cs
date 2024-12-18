using System.ComponentModel.DataAnnotations;
using API.Shared.Models.DDD;
using Microsoft.AspNetCore.Http.HttpResults;

namespace API.Modules.Identity.Models;

public sealed record UserId(Guid Value);

public class User : Entity<UserId>
{
    // Required by EF Core
    private User()
    {
    }

    public int? CompatId { get; private set; }
    public string Username { get; private set; }
    public string PasswordHash { get; private set; }
    public string Email { get; private set; }

    public static User Create(string username, string email, string passwordHash)
    {
        return new User()
        {
            Id = new UserId(Guid.NewGuid()),
            Username = username,
            Email = email,
            PasswordHash = passwordHash
        };
    }

    public static User CreateCompat(int compatId, string username, string email, string passwordHash)
    {
        return new User()
        {
            Id = new UserId(Guid.NewGuid()),
            CompatId = compatId,
            Username = username,
            Email = email,
            PasswordHash = passwordHash
        };
    }
}