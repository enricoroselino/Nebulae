namespace API.Modules.Identity.Features.IAM.RoleClaim.AddRoleClaim;

public class AddRoleClaimCommandValidator : AbstractValidator<AddRoleClaimCommand>
{
    public AddRoleClaimCommandValidator()
    {
        RuleFor(c => c.ClaimType).NotEmpty();
        RuleFor(c => c.ClaimValue).NotEmpty();
    }
}