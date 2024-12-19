namespace API.Modules.Identity.Features.IAM.UserRoles.DeleteUserRoles;

public record DeleteUserRolesCommand(UserId UserId, List<RoleId> RoleIds) : ICommand<Result>;

public class DeleteRoleFromUserCommandHandler : ICommandHandler<DeleteUserRolesCommand, Result>
{
    private readonly AppIdentityDbContext _dbContext;
    private readonly IIdentityRepository _identityRepository;

    public DeleteRoleFromUserCommandHandler(AppIdentityDbContext dbContext, IIdentityRepository identityRepository)
    {
        _dbContext = dbContext;
        _identityRepository = identityRepository;
    }

    public async Task<Result> Handle(DeleteUserRolesCommand request, CancellationToken cancellationToken)
    {
        var isUserExists = await _identityRepository.CheckUserExist(request.UserId, cancellationToken);
        if (!isUserExists) return Result.NotFound("User not found");

        var rolesToRemove = await _dbContext.UserRoles
            .AsNoTracking()
            .Include(c => c.Role)
            .Where(c =>
                c.UserId == request.UserId &&
                request.RoleIds.Contains(c.RoleId))
            .ToListAsync(cancellationToken);

        if (rolesToRemove.Count == 0) return Result.NoContent();

        _dbContext.UserRoles.RemoveRange(rolesToRemove);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result.NoContent();
    }
}