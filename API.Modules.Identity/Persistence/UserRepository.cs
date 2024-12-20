using System.Security.Claims;
using API.Modules.Identity.Dtos;
using API.Modules.Identity.Persistence.Specifications;
using API.Shared.Models;
using Ardalis.Specification.EntityFrameworkCore;
using Shared.Helpers;

namespace API.Modules.Identity.Persistence;

public interface IUserRepository : IUnitOfWork
{
    public DbSet<User> Users { get; }
    public DbSet<UserRole> UserRoles { get; }
    public DbSet<UserClaim> UserClaims { get; }

    public Task<User?> GetUser(
        UserId? userId = null,
        string? username = null,
        string? email = null,
        CancellationToken cancellationToken = default);

    public Task<bool> IsUserExist(
        UserId? userId = null,
        string? username = null,
        string? email = null,
        CancellationToken cancellationToken = default);

    public Task<List<Claim>> GetUserClaims(UserId userId, CancellationToken cancellationToken = default);
}

public class UserRepository : IUserRepository
{
    private readonly AppIdentityDbContext _dbContext;

    public DbSet<User> Users => _dbContext.Users;
    public DbSet<UserRole> UserRoles => _dbContext.UserRoles;
    public DbSet<UserClaim> UserClaims => _dbContext.UserClaims;

    public UserRepository(AppIdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetUser(
        UserId? userId = null,
        string? username = null,
        string? email = null,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .WithSpecification(new GetUserSpecification(userId, username, email))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> IsUserExist(
        UserId? userId = null,
        string? username = null,
        string? email = null,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .WithSpecification(new GetUserSpecification(userId, username, email))
            .AnyAsync(cancellationToken);
    }

    public async Task<List<Claim>> GetUserClaims(UserId userId, CancellationToken cancellationToken = default)
    {
        var userClaims = await _dbContext.Users
            .AsNoTracking()
            .Include(c => c.UserClaims)
            .WithSpecification(new GetUserSpecification(userId: userId))
            .SelectMany(c => c.UserClaims)
            .Select(uc => new Claim(uc.ClaimType, uc.ClaimValue))
            .ToListAsync(cancellationToken);

        var userRoles = await _dbContext.UserRoles
            .AsNoTracking()
            .Include(c => c.Role)
            .Where(c => c.UserId == userId)
            .Select(ur => new Claim("Roles", ur.Role.Name))
            .ToListAsync(cancellationToken);

        var roleClaims = await _dbContext.UserRoles
            .AsNoTracking()
            .Include(c => c.Role)
            .ThenInclude(cc => cc.RoleClaims)
            .Where(c => c.UserId == userId)
            .SelectMany(c => c.Role.RoleClaims)
            .Select(ur => new Claim(ur.ClaimType, ur.ClaimValue))
            .ToListAsync(cancellationToken);

        return ListHelper.Merge(userClaims, userRoles, roleClaims);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}