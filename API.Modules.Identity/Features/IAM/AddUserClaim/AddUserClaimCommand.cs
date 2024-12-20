namespace API.Modules.Identity.Features.IAM.AddUserClaim;

public record AddUserClaimCommand(UserId UserId, string ClaimType, string ClaimValue) : ICommand<Result>;

public class AddUserClaimCommandHandler : ICommandHandler<AddUserClaimCommand, Result>
{
    private readonly IUserRepository _userRepository;

    public AddUserClaimCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(AddUserClaimCommand request, CancellationToken cancellationToken)
    {
        var isUserExists =
            await _userRepository.IsUserExist(userId: request.UserId, cancellationToken: cancellationToken);
        if (!isUserExists) return Result.NotFound("User not found");

        var isClaimExists = await _userRepository.UserClaims
            .AnyAsync(c =>
                c.UserId == request.UserId &&
                c.ClaimType == request.ClaimType &&
                c.ClaimValue == request.ClaimValue, cancellationToken);

        if (isClaimExists) return Result.Conflict("Claim already exists.");

        var newUserClaim = Models.UserClaim.Create(request.UserId, request.ClaimType, request.ClaimValue);
        await _userRepository.UserClaims.AddAsync(newUserClaim, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}