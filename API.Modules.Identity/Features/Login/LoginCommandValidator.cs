using System.Text.RegularExpressions;
using API.Modules.Identity.Constants;

namespace API.Modules.Identity.Features.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(c => c.Username).NotEmpty();
        RuleFor(c => c.Password).NotEmpty();
    }
}