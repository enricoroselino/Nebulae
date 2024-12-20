namespace API.Modules.Identity.Features.IAM.AddRoleClaim;

public record AddRoleClaimCommand(RoleId RoleId, string ClaimType, string ClaimValue) : ICommand<Result>;

public class AddRoleClaimCommandHandler : ICommandHandler<AddRoleClaimCommand, Result>
{
    private readonly IRoleRepository _roleRepository;

    public AddRoleClaimCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<Result> Handle(AddRoleClaimCommand request, CancellationToken cancellationToken)
    {
        var requestedRoles = await _roleRepository.GetMatchingRoleIds([request.RoleId], cancellationToken);
        if (requestedRoles.Count == 0) return Result.NotFound("Role Id does not exist.");

        var isClaimExists = await _roleRepository.RoleClaims
            .AnyAsync(c =>
                c.RoleId == request.RoleId &&
                c.ClaimType == request.ClaimType &&
                c.ClaimValue == request.ClaimValue, cancellationToken);

        if (isClaimExists) return Result.Conflict("Claim already exists.");

        var newRoleClaim = Models.RoleClaim.Create(request.RoleId, request.ClaimType, request.ClaimValue);
        await _roleRepository.RoleClaims.AddAsync(newRoleClaim, cancellationToken);
        await _roleRepository.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}