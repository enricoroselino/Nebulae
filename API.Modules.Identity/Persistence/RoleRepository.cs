using API.Shared.Models;

namespace API.Modules.Identity.Persistence;

public interface IRoleRepository : IUnitOfWork
{
    public DbSet<Role> Roles { get; }
    public DbSet<RoleClaim> RoleClaims { get; }
    public DbSet<UserRole> UserRoles { get; }

    public Task<List<RoleId>> GetMatchingRoleIds(
        List<RoleId> roleIdsRequest,
        CancellationToken cancellationToken = default);
}

public class RoleRepository : IRoleRepository
{
    private readonly AppIdentityDbContext _context;
    public DbSet<Role> Roles => _context.Roles;
    public DbSet<RoleClaim> RoleClaims => _context.RoleClaims;
    public DbSet<UserRole> UserRoles => _context.UserRoles;

    public RoleRepository(AppIdentityDbContext context)
    {
        _context = context;
    }

    public async Task<List<RoleId>> GetMatchingRoleIds(
        List<RoleId> roleIdsRequest,
        CancellationToken cancellationToken = default)
    {
        return await _context.Roles
            .AsNoTracking()
            .Where(c => roleIdsRequest.Contains(c.Id))
            .Select(c => c.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}