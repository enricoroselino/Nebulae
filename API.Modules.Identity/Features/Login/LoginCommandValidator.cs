namespace API.Modules.Identity.Features.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(command => command.Username).NotEmpty();
        RuleFor(command => command.Password).NotEmpty();
    }
}