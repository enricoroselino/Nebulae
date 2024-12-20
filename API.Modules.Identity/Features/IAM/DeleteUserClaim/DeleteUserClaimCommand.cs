namespace API.Modules.Identity.Features.IAM.DeleteUserClaim;

public record DeleteUserClaimCommand(List<UserClaimId> UserClaimIds) : ICommand<Result>;

public class DeleteUserClaimCommandHandler : ICommandHandler<DeleteUserClaimCommand, Result>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserClaimCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(DeleteUserClaimCommand request, CancellationToken cancellationToken)
    {
        var claimsToRemove = await _userRepository.UserClaims
            .AsNoTracking()
            .Where(c => request.UserClaimIds.Contains(c.Id))
            .ToListAsync(cancellationToken);

        if (claimsToRemove.Count == 0) return Result.NoContent();

        _userRepository.UserClaims.RemoveRange(claimsToRemove);
        await _userRepository.SaveChangesAsync(cancellationToken);
        return Result.NoContent();
    }
}