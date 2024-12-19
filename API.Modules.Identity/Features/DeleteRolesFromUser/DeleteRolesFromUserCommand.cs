using API.Shared.Models.CQRS;

namespace API.Modules.Identity.Features.DeleteRolesFromUser;

public record DeleteRolesFromUserCommand(UserId UserId, List<RoleId> RoleIds) : ICommand<Result>;

public class DeleteRoleFromUserCommandHandler : ICommandHandler<DeleteRolesFromUserCommand, Result>
{
    private readonly AppIdentityDbContext _dbContext;
    private readonly IIdentityRepository _identityRepository;

    public DeleteRoleFromUserCommandHandler(AppIdentityDbContext dbContext, IIdentityRepository identityRepository)
    {
        _dbContext = dbContext;
        _identityRepository = identityRepository;
    }

    public async Task<Result> Handle(DeleteRolesFromUserCommand request, CancellationToken cancellationToken)
    {
        var isUserExists = await _identityRepository.CheckUserExist(request.UserId, cancellationToken);
        if (!isUserExists) return Result.NotFound("User not found");

        var requestedRoles = await _identityRepository.GetMatchingRoleIds(request.RoleIds, cancellationToken);
        if (requestedRoles.Count == 0) return Result.NotFound("Roles not found");

        var rolesToRemove = await _dbContext.UserRoles
            .AsNoTracking()
            .Include(c => c.Role)
            .Where(c =>
                c.UserId == request.UserId &&
                requestedRoles.Contains(c.RoleId))
            .ToListAsync(cancellationToken);

        if (rolesToRemove.Count == 0) return Result.Success();

        _dbContext.UserRoles.RemoveRange(rolesToRemove);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}