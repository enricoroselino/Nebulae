namespace API.Modules.Identity.Features.IAM.RoleClaim.DeleteRoleClaim;

public class DeleteRoleClaimCommandValidator : AbstractValidator<DeleteRoleClaimCommand>
{
    public DeleteRoleClaimCommandValidator()
    {
        RuleForEach(r => r.RoleClaimIds).NotNull();
    }
}