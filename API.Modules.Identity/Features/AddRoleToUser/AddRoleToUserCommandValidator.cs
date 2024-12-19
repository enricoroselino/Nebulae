using FluentValidation;

namespace API.Modules.Identity.Features.AddRoleToUser;

public class AddRoleToUserCommandValidator : AbstractValidator<AddRoleToUserCommand>
{
    public AddRoleToUserCommandValidator()
    {
        RuleFor(u => u.UserId).NotNull();
        RuleForEach(c => c.RoleIds).NotEmpty();
    }
}