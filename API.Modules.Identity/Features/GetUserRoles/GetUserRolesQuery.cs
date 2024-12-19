using API.Modules.Identity.Dtos;
using API.Shared.Models.CQRS;

namespace API.Modules.Identity.Features.GetUserRoles;

public record GetUserRolesQuery(UserId UserId) : IQuery<Result<List<RoleDto>>>;

public class GetUserRolesQueryHandler : IQueryHandler<GetUserRolesQuery, Result<List<RoleDto>>>
{
    private readonly AppIdentityDbContext _dbContext;

    public GetUserRolesQueryHandler(AppIdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<RoleDto>>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        var userRoles = await _dbContext.UserRoles
            .AsNoTracking()
            .Include(c => c.Role)
            .Where(c => c.UserId == request.UserId)
            .Select(c => RoleDto.Create(c.Role))
            .ToListAsync(cancellationToken);

        return Result.Success(userRoles);
    }
}