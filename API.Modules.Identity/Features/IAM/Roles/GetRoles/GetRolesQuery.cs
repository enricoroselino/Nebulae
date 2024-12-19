using API.Modules.Identity.Dtos;

namespace API.Modules.Identity.Features.IAM.Roles.GetRoles;

public record GetRolesQuery() : IQuery<Result<List<RoleDto>>>;

public class GetRolesQueryHandler : IQueryHandler<GetRolesQuery, Result<List<RoleDto>>>
{
    private readonly AppIdentityDbContext _dbContext;

    public GetRolesQueryHandler(AppIdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<RoleDto>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _dbContext.Roles
            .AsNoTracking()
            .Select(c => RoleDto.Create(c))
            .ToListAsync(cancellationToken);

        return Result.Success(roles);
    }
}