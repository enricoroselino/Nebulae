namespace API.Modules.Identity.Features.IAM.AddUserRoles;

public class AddUserRolesCommandValidator : AbstractValidator<AddUserRolesCommand>
{
    public AddUserRolesCommandValidator()
    {
        RuleFor(u => u.UserId).NotNull();
        RuleForEach(c => c.RoleIds).NotEmpty();
    }
}