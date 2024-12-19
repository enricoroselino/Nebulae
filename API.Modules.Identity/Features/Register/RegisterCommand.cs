using API.Shared.Models.CQRS;
using Ardalis.Result;
using Shared.Utilities.Hasher;

namespace API.Modules.Identity.Features.Register;

public record RegisterCommand(
    string Username,
    string Password,
    string PasswordConfirm,
    string Email,
    string Fullname
) : ICommand<Result>;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand, Result>
{
    private readonly AppIdentityDbContext _context;
    private readonly IHasher _hasher;

    public RegisterCommandHandler(AppIdentityDbContext context, IHasher hasher)
    {
        _context = context;
        _hasher = hasher;
    }

    public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var exists = await _context.Users.AnyAsync(x => x.Username == request.Username, cancellationToken);
        if (exists) return Result.Conflict("Username is already taken");

        var hashedPassword = _hasher.Hash(request.Password);
        var newUser = User.Create(request.Username, request.Email, request.Fullname, hashedPassword);
        await _context.Users.AddAsync(newUser, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}