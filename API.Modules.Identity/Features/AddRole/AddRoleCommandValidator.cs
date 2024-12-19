namespace API.Modules.Identity.Features.AddRole;

public class AddRoleCommandValidator : AbstractValidator<AddRoleCommand>
{
    public AddRoleCommandValidator()
    {
        RuleFor(c => c.RoleName).NotEmpty();
    }
}