namespace API.Modules.Identity.Features.IAM.Roles.AddRole;

public class AddRoleCommandValidator : AbstractValidator<AddRoleCommand>
{
    public AddRoleCommandValidator()
    {
        RuleFor(c => c.RoleName)
            .NotEmpty()
            .MaximumLength(50);
    }
}