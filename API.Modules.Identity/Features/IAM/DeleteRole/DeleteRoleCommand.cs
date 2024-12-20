namespace API.Modules.Identity.Features.IAM.DeleteRole;

public record DeleteRoleCommand(RoleId RoleId) : ICommand<Result>;

public class DeleteRoleCommandHandler : ICommandHandler<DeleteRoleCommand, Result>
{
    private readonly IRoleRepository _roleRepository;

    public DeleteRoleCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.Roles.FindAsync([request.RoleId], cancellationToken);
        if (role is null) return Result.NoContent();

        _roleRepository.Roles.Remove(role);
        await _roleRepository.SaveChangesAsync(cancellationToken);
        return Result.NoContent();
    }
}