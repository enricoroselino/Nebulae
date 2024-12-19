using API.Shared.Models.CQRS;
using Ardalis.Result;

namespace API.Modules.Identity.Features.AddRolesToUser;

public record AddRolesToUserCommand(UserId UserId, List<RoleId> RoleIds) : ICommand<Result>;

public class AddRoleToUserCommandHandler : ICommandHandler<AddRolesToUserCommand, Result>
{
    private readonly AppIdentityDbContext _dbContext;
    private readonly IIdentityRepository _identityRepository;

    public AddRoleToUserCommandHandler(AppIdentityDbContext dbContext, IIdentityRepository identityRepository)
    {
        _dbContext = dbContext;
        _identityRepository = identityRepository;
    }

    public async Task<Result> Handle(AddRolesToUserCommand request, CancellationToken cancellationToken)
    {
        var isUserExists = await _identityRepository.CheckUserExist(request.UserId, cancellationToken);
        if (!isUserExists) return Result.NotFound("User not found");

        var requestedRoles = await _identityRepository.GetMatchingRoleIds(request.RoleIds, cancellationToken);
        if (requestedRoles.Count == 0) return Result.NotFound("Roles not found");

        var assignedRoles = await _dbContext.UserRoles
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
        await _dbContext.UserRoles.AddRangeAsync(newUserRoles, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}