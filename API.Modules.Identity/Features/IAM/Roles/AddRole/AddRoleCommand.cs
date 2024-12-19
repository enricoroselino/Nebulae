using Humanizer;

namespace API.Modules.Identity.Features.IAM.Roles.AddRole;

public record AddRoleCommand(string RoleName) : ICommand<Result>;

public class AddRoleCommandHandler : ICommandHandler<AddRoleCommand, Result>
{
    private readonly AppIdentityDbContext _dbContext;

    public AddRoleCommandHandler(AppIdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> Handle(AddRoleCommand request, CancellationToken cancellationToken)
    {
        var exists = await _dbContext.Roles
            .AnyAsync(c => EF.Functions.Like(c.Name, request.RoleName), cancellationToken);

        if (exists) return Result.Conflict("Role already exists");

        _dbContext.Roles.Add(Role.Create(request.RoleName.Titleize()));
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}