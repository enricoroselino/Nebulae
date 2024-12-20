namespace API.Modules.Identity.Features.IAM.AddUserClaim;

public class AddUserClaimCommandValidator : AbstractValidator<AddUserClaimCommand>
{
    public AddUserClaimCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();
        RuleFor(c => c.ClaimType).NotEmpty();
        RuleFor(c => c.ClaimValue).NotEmpty();
    }
}