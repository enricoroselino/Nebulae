namespace API.Modules.Identity.Features.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(c => c.Username).NotEmpty();
        RuleFor(c => c.Password).NotEmpty();
        RuleFor(c => c.PasswordConfirm)
            .NotEmpty()
            .Equal(c => c.Password);
        RuleFor(c => c.Email).NotEmpty();
        RuleFor(c => c.Fullname).NotEmpty();
    }
}