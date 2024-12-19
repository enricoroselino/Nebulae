namespace API.Modules.Identity.Features.IAM.RoleClaim.DeleteRoleClaim;

public record DeleteRoleClaimCommand(List<RoleClaimId> RoleClaimIds) : ICommand<Result>;

public class DeleteRoleClaimCommandHandler : ICommandHandler<DeleteRoleClaimCommand, Result>
{
    private readonly AppIdentityDbContext _dbContext;

    public DeleteRoleClaimCommandHandler(AppIdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(DeleteRoleClaimCommand request, CancellationToken cancellationToken)
    {
        var claimsToRemove = await _dbContext.RoleClaims
            .AsNoTracking()
            .Where(c => request.RoleClaimIds.Contains(c.Id))
            .ToListAsync(cancellationToken);

        if (claimsToRemove.Count == 0) return Result.NoContent();
        
        _dbContext.RoleClaims.RemoveRange(claimsToRemove);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result.NoContent();
    }
}