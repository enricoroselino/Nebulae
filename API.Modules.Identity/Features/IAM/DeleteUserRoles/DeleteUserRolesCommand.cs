namespace API.Modules.Identity.Features.IAM.DeleteUserRoles;

public record DeleteUserRolesCommand(UserId UserId, List<RoleId> RoleIds) : ICommand<Result>;

public class DeleteRoleFromUserCommandHandler : ICommandHandler<DeleteUserRolesCommand, Result>
{
    private readonly IUserRepository _userRepository;

    public DeleteRoleFromUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(DeleteUserRolesCommand request, CancellationToken cancellationToken)
    {
        var isUserExists =
            await _userRepository.IsUserExist(userId: request.UserId, cancellationToken: cancellationToken);
        if (!isUserExists) return Result.NotFound("User not found");

        var rolesToRemove = await _userRepository.UserRoles
            .AsNoTracking()
            .Include(c => c.Role)
            .Where(c =>
                c.UserId == request.UserId &&
                request.RoleIds.Contains(c.RoleId))
            .ToListAsync(cancellationToken);

        if (rolesToRemove.Count == 0) return Result.NoContent();

        _userRepository.UserRoles.RemoveRange(rolesToRemove);
        await _userRepository.SaveChangesAsync(cancellationToken);
        return Result.NoContent();
    }
}