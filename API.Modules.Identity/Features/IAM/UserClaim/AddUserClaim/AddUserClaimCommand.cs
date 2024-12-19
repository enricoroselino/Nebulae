namespace API.Modules.Identity.Features.IAM.UserClaim.AddUserClaim;

public record AddUserClaimCommand(UserId UserId, string ClaimType, string ClaimValue) : ICommand<Result>;

public class AddUserClaimCommandHandler : ICommandHandler<AddUserClaimCommand, Result>
{
    private readonly AppIdentityDbContext _dbContext;
    private readonly IIdentityRepository _identityRepository;

    public AddUserClaimCommandHandler(AppIdentityDbContext dbContext, IIdentityRepository identityRepository)
    {
        _dbContext = dbContext;
        _identityRepository = identityRepository;
    }

    public async Task<Result> Handle(AddUserClaimCommand request, CancellationToken cancellationToken)
    {
        var isUserExists = await _identityRepository.CheckUserExist(request.UserId, cancellationToken);
        if (!isUserExists) return Result.NotFound("User not found");

        var isClaimExists = await _dbContext.UserClaims
            .AnyAsync(c =>
                c.UserId == request.UserId &&
                c.ClaimType == request.ClaimType &&
                c.ClaimValue == request.ClaimValue, cancellationToken);

        if (isClaimExists) return Result.Conflict("Claim already exists.");

        var newUserClaim = Models.UserClaim.Create(request.UserId, request.ClaimType, request.ClaimValue);
        await _dbContext.UserClaims.AddAsync(newUserClaim, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}