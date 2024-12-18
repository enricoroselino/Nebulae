using API.Shared.Models.CQRS;
using Ardalis.Result;
using FluentValidation;
using Humanizer;

namespace API.Modules.Identity.Features.AddRole;

public record AddRoleCommand(string RoleName) : ICommand<Result>;

public class AddRoleCommandValidator : AbstractValidator<AddRoleCommand>
{
    public AddRoleCommandValidator()
    {
        RuleFor(c => c.RoleName).NotEmpty();
    }
}

public class AddRoleCommandHandler : ICommandHandler<AddRoleCommand, Result>
{
    private readonly AppIdentityDbContext _context;

    public AddRoleCommandHandler(AppIdentityDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(AddRoleCommand request, CancellationToken cancellationToken)
    {
        var exists = await _context.Roles
            .AnyAsync(c => EF.Functions.Like(c.Name, request.RoleName), cancellationToken);

        if (exists) return Result.Success();

        _context.Roles.Add(Role.Create(request.RoleName.Titleize()));
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}