namespace API.Modules.Identity.Persistence;

public interface IIdentityRepository
{
    public Task<bool> CheckUserExist(UserId userId, CancellationToken cancellationToken = default);

    public Task<List<RoleId>> GetMatchingRoleIds(List<RoleId> roleIdsRequest,
        CancellationToken cancellationToken = default);
}

public class IdentityRepository : IIdentityRepository
{
    private readonly AppIdentityDbContext _context;

    public IdentityRepository(AppIdentityDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CheckUserExist(UserId userId, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AsNoTracking()
            .AnyAsync(u => u.Id == userId, cancellationToken);
    }

    public async Task<List<RoleId>> GetMatchingRoleIds(
        List<RoleId> roleIdsRequest,
        CancellationToken cancellationToken = default)
    {
        var roles = await _context.Roles
            .AsNoTracking()
            .Where(c => roleIdsRequest.Contains(c.Id))
            .Select(c => c.Id)
            .ToListAsync(cancellationToken);

        return roles;
    }
}