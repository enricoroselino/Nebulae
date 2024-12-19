using FluentValidation;

namespace API.Modules.Identity.Features.AddRolesToUser;

public class AddRolesToUserCommandValidator : AbstractValidator<AddRolesToUserCommand>
{
    public AddRolesToUserCommandValidator()
    {
        RuleFor(u => u.UserId).NotNull();
        RuleForEach(c => c.RoleIds).NotEmpty();
    }
}