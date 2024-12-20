﻿namespace API.Modules.Identity.Models;

public sealed record UserId(Guid Value);

public class User : Aggregate<UserId>
{
    private User()
    {
    }

    public int? CompatId { get; private set; }
    public string Username { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Fullname { get; private set; } = string.Empty;
    public DateTime? LastLogin { get; private set; }

    // Navigation
    public virtual ICollection<UserRole> UserRoles { get; private set; } = null!;
    public virtual ICollection<UserClaim> UserClaims { get; private set; } = null!;

    public static User Create(string username, string email, string fullname, string passwordHash)
    {
        return new User()
        {
            Id = new UserId(Guid.NewGuid()),
            Username = username,
            Email = email,
            PasswordHash = passwordHash,
            Fullname = fullname,
        };
    }

    public static User CreateCompat(int compatId, string username, string email, string fullname, string passwordHash)
    {
        return new User()
        {
            Id = new UserId(Guid.NewGuid()),
            CompatId = compatId,
            Username = username,
            Email = email,
            PasswordHash = passwordHash,
            Fullname = fullname,
        };
    }

    public void LoggedIn()
    {
        LastLogin = DateTime.UtcNow;
    }
}