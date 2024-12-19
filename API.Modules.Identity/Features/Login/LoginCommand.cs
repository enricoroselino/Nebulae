using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using API.Shared.Models.CQRS;
using API.Shared.Utilities.TokenProvider;
using Ardalis.Result;
using Shared.Models;
using Shared.Utilities.Hasher;

namespace API.Modules.Identity.Features.Login;

public record LoginCommand(string Username, string Password) : ICommand<Result<AuthToken>>;

public class LoginAppCommandHandler : ICommandHandler<LoginCommand, Result<AuthToken>>
{
    private readonly AppIdentityDbContext _dbContext;
    private readonly IAuthTokenProvider _tokenProvider;
    private readonly IHasher _hasher;

    public LoginAppCommandHandler(AppIdentityDbContext dbContext, IAuthTokenProvider tokenProvider, IHasher hasher)
    {
        _dbContext = dbContext;
        _tokenProvider = tokenProvider;
        _hasher = hasher;
    }

    public async Task<Result<AuthToken>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(user => user.Username == request.Username, cancellationToken);

        if (user is null || !_hasher.VerifyHash(request.Password, user.PasswordHash)) return Result.Unauthorized();

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.Value.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.Fullname),
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
        };

        var authToken = _tokenProvider.Create(claims, "Nebulae");
        return Result.Success(authToken);
    }
}