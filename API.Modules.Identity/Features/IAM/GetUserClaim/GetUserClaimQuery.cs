using API.Modules.Identity.Dtos;

namespace API.Modules.Identity.Features.IAM.GetUserClaim;

public record GetUserClaimQuery(UserId UserId) : IQuery<Result<List<ClaimDto>>>;

public class GetUserClaimQueryHandler : IQueryHandler<GetUserClaimQuery, Result<List<ClaimDto>>>
{
    private readonly IUserRepository _userRepository;

    public GetUserClaimQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<List<ClaimDto>>> Handle(GetUserClaimQuery request, CancellationToken cancellationToken)
    {
        var claims = await _userRepository.GetUserClaims(request.UserId, cancellationToken);
        return Result.Success(claims.Select(x => new ClaimDto(x.Type, x.Value)).ToList());
    }
}