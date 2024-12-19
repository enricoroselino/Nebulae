using API.Modules.Identity.Dtos;

namespace API.Modules.Identity.Features.IAM.Users.GetUsers;

public record GetUsersQuery() : IQuery<Result<List<UserDto>>>;

public class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, Result<List<UserDto>>>
{
    private readonly AppIdentityDbContext _dbContext;

    public GetUsersQueryHandler(AppIdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _dbContext.Users
            .AsNoTracking()
            .Select(c => UserDto.Create(c))
            .ToListAsync(cancellationToken);

        return Result.Success(users);
    }
}