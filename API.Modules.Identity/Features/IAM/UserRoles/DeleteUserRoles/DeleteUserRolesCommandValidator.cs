namespace API.Modules.Identity.Features.IAM.UserRoles.DeleteUserRoles;

public class DeleteUserRolesCommandValidator : AbstractValidator<DeleteUserRolesCommand>
{
    public DeleteUserRolesCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();
        RuleForEach(c => c.RoleIds).NotEmpty();
    }
}