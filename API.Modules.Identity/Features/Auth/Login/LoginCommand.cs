using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using API.Shared.Utilities.TokenProvider;
using Microsoft.Extensions.Options;
using Shared.Helpers;
using Shared.Models;
using Shared.Utilities.Hasher;

namespace API.Modules.Identity.Features.Auth.Login;

public record LoginCommand(string Username, string Password) : ICommand<Result<AuthToken>>;

public class LoginAppCommandHandler : ICommandHandler<LoginCommand, Result<AuthToken>>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthTokenProvider _tokenProvider;
    private readonly IHasher _hasher;
    private readonly AuthTokenProviderOptions _options;

    public LoginAppCommandHandler(
        IAuthTokenProvider tokenProvider,
        IUserRepository userRepository,
        IHasher hasher,
        IOptions<AuthTokenProviderOptions> options)
    {
        _tokenProvider = tokenProvider;
        _hasher = hasher;
        _userRepository = userRepository;
        _options = options.Value;
    }

    public async Task<Result<AuthToken>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUser(username: request.Username, cancellationToken: cancellationToken);
        if (user is null || !_hasher.VerifyHash(request.Password, user.PasswordHash)) return Result.Unauthorized();

        user.LoggedIn();
        await _userRepository.SaveChangesAsync(cancellationToken);

        var defaultClaims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.Value.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.Fullname),
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
        };

        var authToken = _tokenProvider.Create(defaultClaims, _options.ValidAudience);
        return Result.Success(authToken);
    }
}