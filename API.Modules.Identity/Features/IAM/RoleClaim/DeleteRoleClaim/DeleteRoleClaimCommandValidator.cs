namespace API.Modules.Identity.Features.IAM.RoleClaim.DeleteRoleClaim;

public class DeleteRoleClaimCommandValidator : AbstractValidator<DeleteRoleClaimCommand>
{
    public DeleteRoleClaimCommandValidator()
    {
        RuleFor(r => r.RoleClaimIds).NotNull();
    }
}