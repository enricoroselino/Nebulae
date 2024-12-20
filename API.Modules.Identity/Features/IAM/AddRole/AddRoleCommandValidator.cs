namespace API.Modules.Identity.Features.IAM.AddRole;

public class AddRoleCommandValidator : AbstractValidator<AddRoleCommand>
{
    public AddRoleCommandValidator()
    {
        RuleFor(c => c.RoleName)
            .NotEmpty()
            .MaximumLength(50);
    }
}