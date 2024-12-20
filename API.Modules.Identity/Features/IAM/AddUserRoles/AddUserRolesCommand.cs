namespace API.Modules.Identity.Features.IAM.AddUserRoles;

public record AddUserRolesCommand(UserId UserId, List<RoleId> RoleIds) : ICommand<Result>;

public class AddRoleToUserCommandHandler : ICommandHandler<AddUserRolesCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;

    public AddRoleToUserCommandHandler(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        AppIdentityDbContext dbContext)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public async Task<Result> Handle(AddUserRolesCommand request, CancellationToken cancellationToken)
    {
        var isUserExists =
            await _userRepository.IsUserExist(userId: request.UserId, cancellationToken: cancellationToken);
        if (!isUserExists) return Result.NotFound("User not found");

        var requestedRoles = await _roleRepository.GetMatchingRoleIds(request.RoleIds, cancellationToken);
        if (requestedRoles.Count == 0) return Result.NotFound("Roles not found");

        var assignedRoles = await _roleRepository.UserRoles
            .AsNoTracking()
            .Include(c => c.Role)
            .Where(c => c.UserId == request.UserId)
            .Select(c => c.Role.Id)
            .ToListAsync(cancellationToken);

        var rolesToAssign = requestedRoles
            .Except(assignedRoles)
            .ToList();

        if (rolesToAssign.Count == 0) return Result.Success();

        var newUserRoles = rolesToAssign.Select(c => UserRole.Create(request.UserId, c));
        await _roleRepository.UserRoles.AddRangeAsync(newUserRoles, cancellationToken);
        await _roleRepository.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}