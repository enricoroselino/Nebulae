using API.Modules.Identity.Dtos;

namespace API.Modules.Identity.Features.IAM.GetUserById;

public record GetUserByIdQuery(UserId UserId) : IQuery<Result<UserDto>>;

public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, Result<UserDto>>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUser(userId: request.UserId, cancellationToken: cancellationToken);
        return user is null ? Result.NotFound() : Result.Success(UserDto.Create(user));
    }
}