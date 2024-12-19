using API.Shared.Models.CQRS;
using Ardalis.Result;

namespace API.Modules.Identity.Features.AddRoleToUser;

public record AddRoleToUserCommand(UserId UserId, List<RoleId> RoleIds) : ICommand<Result>;

public class AddRoleToUserCommandHandler : ICommandHandler<AddRoleToUserCommand, Result>
{
    private readonly AppIdentityDbContext _context;
    private readonly IIdentityRepository _identityRepository;

    public AddRoleToUserCommandHandler(AppIdentityDbContext context, IIdentityRepository identityRepository)
    {
        _context = context;
        _identityRepository = identityRepository;
    }

    public async Task<Result> Handle(AddRoleToUserCommand request, CancellationToken cancellationToken)
    {
        var isUserExists = await _identityRepository.CheckUserExist(request.UserId, cancellationToken);
        if (!isUserExists) return Result.NotFound("User not found");

        var requestedRoles = await _identityRepository.GetMatchingRoleIds(request.RoleIds, cancellationToken);
        if (requestedRoles.Count == 0) return Result.NotFound("Roles not found");

        var assignedRoles = await _context.UserRoles
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
        await _context.UserRoles.AddRangeAsync(newUserRoles, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}