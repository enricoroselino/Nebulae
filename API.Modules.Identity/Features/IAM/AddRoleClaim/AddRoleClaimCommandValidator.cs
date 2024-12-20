namespace API.Modules.Identity.Features.IAM.AddRoleClaim;

public class AddRoleClaimCommandValidator : AbstractValidator<AddRoleClaimCommand>
{
    public AddRoleClaimCommandValidator()
    {
        RuleFor(c => c.RoleId).NotEmpty();
        RuleFor(c => c.ClaimType).NotEmpty();
        RuleFor(c => c.ClaimValue).NotEmpty();
    }
}