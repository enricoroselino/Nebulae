namespace API.Modules.Identity.Features.IAM.RoleClaim.AddRoleClaim;

public record AddRoleClaimCommand(RoleId RoleId, string ClaimType, string ClaimValue) : ICommand<Result>;

public class AddRoleClaimCommandHandler : ICommandHandler<AddRoleClaimCommand, Result>
{
    private readonly AppIdentityDbContext _dbContext;
    private readonly IIdentityRepository _identityRepository;

    public AddRoleClaimCommandHandler(AppIdentityDbContext dbContext, IIdentityRepository identityRepository)
    {
        _dbContext = dbContext;
        _identityRepository = identityRepository;
    }

    public async Task<Result> Handle(AddRoleClaimCommand request, CancellationToken cancellationToken)
    {
        var requestedRoles = await _identityRepository.GetMatchingRoleIds([request.RoleId], cancellationToken);
        if (requestedRoles.Count == 0) return Result.NotFound("Role Id does not exist.");

        var isClaimExists = await _dbContext.RoleClaims
            .AnyAsync(c =>
                c.RoleId == request.RoleId &&
                c.ClaimType == request.ClaimType &&
                c.ClaimValue == request.ClaimValue, cancellationToken);

        if (isClaimExists) return Result.Conflict("Claim already exists.");

        var newRoleClaim = Models.RoleClaim.Create(request.RoleId, request.ClaimType, request.ClaimValue);
        await _dbContext.RoleClaims.AddAsync(newRoleClaim, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}