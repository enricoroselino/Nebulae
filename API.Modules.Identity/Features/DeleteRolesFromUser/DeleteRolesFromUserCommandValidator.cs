namespace API.Modules.Identity.Features.DeleteRolesFromUser;

public class DeleteRolesFromUserCommandValidator : AbstractValidator<DeleteRolesFromUserCommand>
{
    public DeleteRolesFromUserCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();
        RuleForEach(c => c.RoleIds).NotEmpty();
    }
}