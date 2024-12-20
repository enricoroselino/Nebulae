using API.Modules.Identity.Dtos;

namespace API.Modules.Identity.Features.IAM.GetUsers;

public record GetUsersQuery() : IQuery<Result<List<UserDto>>>;

public class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, Result<List<UserDto>>>
{
    private readonly IUserRepository _userRepository;

    public GetUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<List<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.Users
            .AsNoTracking()
            .Select(c => UserDto.Create(c))
            .ToListAsync(cancellationToken);

        return Result.Success(users);
    }
}