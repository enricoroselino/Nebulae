namespace API.Modules.Identity.Features.Auth.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(c => c.Username).NotEmpty();
        RuleFor(c => c.Password).NotEmpty();
    }
}