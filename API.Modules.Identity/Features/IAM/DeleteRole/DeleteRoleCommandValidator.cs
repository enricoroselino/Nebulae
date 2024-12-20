namespace API.Modules.Identity.Features.IAM.DeleteRole;

public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
{
    public DeleteRoleCommandValidator()
    {
        RuleFor(r => r.RoleId).NotNull();
    }
}