using API.Modules.Identity.Dtos;

namespace API.Modules.Identity.Features.IAM.GetUserRoles;

public record GetUserRolesQuery(UserId UserId) : IQuery<Result<List<RoleDto>>>;

public class GetUserRolesQueryHandler : IQueryHandler<GetUserRolesQuery, Result<List<RoleDto>>>
{
    private readonly IUserRepository _userRepository;

    public GetUserRolesQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;

    }

    public async Task<Result<List<RoleDto>>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        var userRoles = await _userRepository.UserRoles
            .AsNoTracking()
            .Include(c => c.Role)
            .Where(c => c.UserId == request.UserId)
            .Select(c => RoleDto.Create(c.Role))
            .ToListAsync(cancellationToken);

        return Result.Success(userRoles);
    }
}