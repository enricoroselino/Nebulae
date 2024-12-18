using API.Shared.Models.CQRS;
using Ardalis.Result;
using FluentValidation;
using Shared.Models;

namespace API.Modules.Identity.Features.LoginApp;

public record LoginAppCommand(string Username, string Password) : ICommand<Result<AuthToken>>;

public class LoginAppCommandValidator : AbstractValidator<LoginAppCommand>
{
    public LoginAppCommandValidator()
    {
        RuleFor(command => command.Username).NotEmpty();
        RuleFor(command => command.Password).NotEmpty();
    }
}

public class LoginAppCommandHandler : ICommandHandler<LoginAppCommand, Result<AuthToken>>
{
    private readonly AppIdentityDbContext _context;

    public LoginAppCommandHandler(AppIdentityDbContext context)
    {
        _context = context;
    }

    public async Task<Result<AuthToken>> Handle(LoginAppCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(user => user.Username == request.Username, cancellationToken);

        if (user is null) return Result.Unauthorized();

        var authToken = new AuthToken("", "");
        return Result<AuthToken>.Success(authToken);
    }
}