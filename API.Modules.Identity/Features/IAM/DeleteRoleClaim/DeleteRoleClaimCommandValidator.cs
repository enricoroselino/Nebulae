namespace API.Modules.Identity.Features.IAM.DeleteRoleClaim;

public class DeleteRoleClaimCommandValidator : AbstractValidator<DeleteRoleClaimCommand>
{
    public DeleteRoleClaimCommandValidator()
    {
        RuleForEach(r => r.RoleClaimIds).NotNull();
    }
}