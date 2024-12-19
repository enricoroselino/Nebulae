namespace API.Modules.Identity.Features.IAM.UserClaim.DeleteUserClaim;

public class DeleteUserClaimCommandValidator : AbstractValidator<DeleteUserClaimCommand>
{
    public DeleteUserClaimCommandValidator()
    {
        RuleForEach(c => c.UserClaimIds).NotEmpty();
    }
}