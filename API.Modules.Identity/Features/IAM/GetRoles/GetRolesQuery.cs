using API.Modules.Identity.Dtos;

namespace API.Modules.Identity.Features.IAM.GetRoles;

public record GetRolesQuery() : IQuery<Result<List<RoleDto>>>;

public class GetRolesQueryHandler : IQueryHandler<GetRolesQuery, Result<List<RoleDto>>>
{
    private readonly IRoleRepository _roleRepository;

    public GetRolesQueryHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<Result<List<RoleDto>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _roleRepository.Roles
            .AsNoTracking()
            .Select(c => RoleDto.Create(c))
            .ToListAsync(cancellationToken);

        return Result.Success(roles);
    }
}