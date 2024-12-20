using Humanizer;

namespace API.Modules.Identity.Features.IAM.AddRole;

public record AddRoleCommand(string RoleName) : ICommand<Result>;

public class AddRoleCommandHandler : ICommandHandler<AddRoleCommand, Result>
{
    private readonly IRoleRepository _roleRepository;

    public AddRoleCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<Result> Handle(AddRoleCommand request, CancellationToken cancellationToken)
    {
        var exists = await _roleRepository.Roles
            .AnyAsync(c => EF.Functions.Like(c.Name, request.RoleName), cancellationToken);

        if (exists) return Result.Conflict("Role already exists");

        _roleRepository.Roles.Add(Role.Create(request.RoleName.Titleize()));
        await _roleRepository.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}