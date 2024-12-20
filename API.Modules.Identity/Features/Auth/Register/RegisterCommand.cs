using Shared.Utilities.Hasher;

namespace API.Modules.Identity.Features.Auth.Register;

public record RegisterCommand(
    string Username,
    string Password,
    string PasswordConfirm,
    string Email,
    string Fullname
) : ICommand<Result>;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IHasher _hasher;

    public RegisterCommandHandler(IHasher hasher, IUserRepository userRepository)
    {
        _hasher = hasher;
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var isUserExist =
            await _userRepository.IsUserExist(username: request.Username, cancellationToken: cancellationToken);
        if (isUserExist) return Result.Conflict("Username is already taken");

        var hashedPassword = _hasher.Hash(request.Password);
        var newUser = User.Create(request.Username, request.Email, request.Fullname, hashedPassword);
        await _userRepository.Users.AddAsync(newUser, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}