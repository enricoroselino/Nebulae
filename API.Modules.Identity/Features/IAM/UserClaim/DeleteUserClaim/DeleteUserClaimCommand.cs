namespace API.Modules.Identity.Features.IAM.UserClaim.DeleteUserClaim;

public record DeleteUserClaimCommand(List<UserClaimId> UserClaimIds) : ICommand<Result>;

public class DeleteUserClaimCommandHandler : ICommandHandler<DeleteUserClaimCommand, Result>
{
    private readonly AppIdentityDbContext _dbContext;

    public DeleteUserClaimCommandHandler(AppIdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(DeleteUserClaimCommand request, CancellationToken cancellationToken)
    {
        var claimsToRemove = await _dbContext.UserClaims
            .AsNoTracking()
            .Where(c => request.UserClaimIds.Contains(c.Id))
            .ToListAsync(cancellationToken);
        
        if (claimsToRemove.Count == 0) return Result.NoContent();
        
        _dbContext.UserClaims.RemoveRange(claimsToRemove);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result.NoContent();
    }
}