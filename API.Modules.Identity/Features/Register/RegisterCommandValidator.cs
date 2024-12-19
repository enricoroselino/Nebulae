using System.Text.RegularExpressions;
using API.Modules.Identity.Constants;

namespace API.Modules.Identity.Features.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(c => c.Username)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(32)
            .Matches(RegexConstant.ValidUsername.Key, RegexOptions.Compiled)
            .WithMessage(RegexConstant.ValidUsername.Value);

        RuleFor(c => c.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(16)
            .Equal(c => c.PasswordConfirm)
            .Matches(RegexConstant.ValidPassword.Key, RegexOptions.Compiled)
            .WithMessage(RegexConstant.ValidPassword.Value);

        RuleFor(c => c.Email)
            .NotEmpty()
            .MaximumLength(320)
            .EmailAddress();

        RuleFor(c => c.Fullname)
            .NotEmpty()
            .MaximumLength(70)
            .Matches(RegexConstant.ValidFullname.Key, RegexOptions.Compiled)
            .WithMessage(RegexConstant.ValidFullname.Value);
    }
}