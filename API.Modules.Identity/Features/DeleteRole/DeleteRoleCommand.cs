using API.Shared.Models.CQRS;
using Ardalis.Result;

namespace API.Modules.Identity.Features.DeleteRole;

public record DeleteRoleCommand(RoleId RoleId) : ICommand<Result>;

public class DeleteRoleCommandHandler : ICommandHandler<DeleteRoleCommand, Result>
{
    private readonly AppIdentityDbContext _context;

    public DeleteRoleCommandHandler(AppIdentityDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _context.Roles.FindAsync([request.RoleId], cancellationToken);
        if (role is null) return Result.Success();

        _context.Roles.Remove(role);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}