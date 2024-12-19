using API.Shared.Models.CQRS;
using Ardalis.Result;

namespace API.Modules.Identity.Features.DeleteRoleFromUser;

public record DeleteRoleFromUserCommand(UserId UserId, List<RoleId> RoleIds) : ICommand<Result>;

public class DeleteRoleFromUserCommandHandler : ICommandHandler<DeleteRoleFromUserCommand, Result>
{
    private readonly AppIdentityDbContext _context;
    private readonly IIdentityRepository _identityRepository;

    public DeleteRoleFromUserCommandHandler(AppIdentityDbContext context, IIdentityRepository identityRepository)
    {
        _context = context;
        _identityRepository = identityRepository;
    }

    public async Task<Result> Handle(DeleteRoleFromUserCommand request, CancellationToken cancellationToken)
    {
        var isUserExists = await _identityRepository.CheckUserExist(request.UserId, cancellationToken);
        if (!isUserExists) return Result.NotFound("User not found");

        var requestedRoles = await _identityRepository.GetMatchingRoleIds(request.RoleIds, cancellationToken);
        if (requestedRoles.Count == 0) return Result.NotFound("Roles not found");

        var rolesToRemove = await _context.UserRoles
            .AsNoTracking()
            .Include(c => c.Role)
            .Where(c =>
                c.UserId == request.UserId &&
                requestedRoles.Contains(c.RoleId))
            .ToListAsync(cancellationToken);

        if (rolesToRemove.Count == 0) return Result.Success();

        _context.UserRoles.RemoveRange(rolesToRemove);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}