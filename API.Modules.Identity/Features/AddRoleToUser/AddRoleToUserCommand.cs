using API.Shared.Models.CQRS;
using Ardalis.Result;
using FluentValidation;

namespace API.Modules.Identity.Features.AddRoleToUser;

public record AddRoleToUserCommand(UserId UserId, List<string> RoleNames) : ICommand<Result>;

public class AddRoleToUserCommandValidator : AbstractValidator<AddRoleToUserCommand>
{
    public AddRoleToUserCommandValidator()
    {
        RuleFor(u => u.UserId)
            .NotNull();

        RuleForEach(c => c.RoleNames)
            .NotEmpty()
            .MaximumLength(50);
    }
}

public class AddRoleToUserCommandHandler : ICommandHandler<AddRoleToUserCommand, Result>
{
    private readonly AppIdentityDbContext _context;

    public AddRoleToUserCommandHandler(AppIdentityDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(AddRoleToUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null) return Result.NotFound("User not found.");

        var requestedRoles = await _context.Roles
            .Where(c => request.RoleNames.Any(x => EF.Functions.Like(c.Name, x)))
            .ToListAsync(cancellationToken);

        var currentRoles = await _context.Users
            .Where(u => u.Id == request.UserId)
            .SelectMany(u => u.UserRoles.Select(ur => ur.Role))
            .ToListAsync(cancellationToken);

        var unassignedUserRoles = requestedRoles
            .Where(r => currentRoles.All(cr => cr.Id != r.Id))
            .Select(c => new UserRole(user.Id, c.Id))
            .ToList();

        if (unassignedUserRoles.Count == 0) return Result.Success();

        await _context.UserRoles.AddRangeAsync(unassignedUserRoles, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}