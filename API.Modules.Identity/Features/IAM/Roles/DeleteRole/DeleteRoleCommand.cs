namespace API.Modules.Identity.Features.IAM.Roles.DeleteRole;

public record DeleteRoleCommand(RoleId RoleId) : ICommand<Result>;

public class DeleteRoleCommandHandler : ICommandHandler<DeleteRoleCommand, Result>
{
    private readonly AppIdentityDbContext _dbContext;

    public DeleteRoleCommandHandler(AppIdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _dbContext.Roles.FindAsync([request.RoleId], cancellationToken);
        if (role is null) return Result.NoContent();

        _dbContext.Roles.Remove(role);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result.NoContent();
    }
}