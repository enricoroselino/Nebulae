namespace API.Modules.Identity.Features.IAM.DeleteRoleClaim;

public record DeleteRoleClaimCommand(List<RoleClaimId> RoleClaimIds) : ICommand<Result>;

public class DeleteRoleClaimCommandHandler : ICommandHandler<DeleteRoleClaimCommand, Result>
{
    private readonly IRoleRepository _roleRepository;

    public DeleteRoleClaimCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<Result> Handle(DeleteRoleClaimCommand request, CancellationToken cancellationToken)
    {
        var claimsToRemove = await _roleRepository.RoleClaims
            .AsNoTracking()
            .Where(c => request.RoleClaimIds.Contains(c.Id))
            .ToListAsync(cancellationToken);

        if (claimsToRemove.Count == 0) return Result.NoContent();

        _roleRepository.RoleClaims.RemoveRange(claimsToRemove);
        await _roleRepository.SaveChangesAsync(cancellationToken);
        return Result.NoContent();
    }
}