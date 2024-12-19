using API.Modules.Identity.Dtos;

namespace API.Modules.Identity.Features.IAM.Users.GetUserById;

public record GetUserByIdQuery(UserId UserId) : IQuery<Result<UserDto>>;

public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, Result<UserDto>>
{
    private readonly AppIdentityDbContext _dbContext;

    public GetUserByIdQueryHandler(AppIdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .SingleOrDefaultAsync(c => c.Id == request.UserId, cancellationToken);

        return user is null ? Result.NotFound() : Result.Success(UserDto.Create(user));
    }
}