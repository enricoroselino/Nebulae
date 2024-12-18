using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Models;

namespace API.Shared.Utilities.TokenProvider;

public sealed class JwtProvider : IAuthTokenProvider
{
    private readonly AuthTokenProviderOptions _options;
    private readonly JwtSecurityTokenHandler _tokenHandler = new();
    private readonly SigningCredentials _signingCredentials;

    public JwtProvider(IOptions<AuthTokenProviderOptions> options)
    {
        _options = options.Value;

        var key = Encoding.UTF8.GetBytes(_options.Key);
        var symmetricSecurityKey = new SymmetricSecurityKey(key);
        _signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512);
    }

    public AuthToken Create(List<Claim> claims, string audience)
    {
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

        var token = new JwtSecurityToken(
            issuer: _options.ValidIssuer,
            audience: audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow,
            signingCredentials: _signingCredentials
        );

        var accessToken = _tokenHandler.WriteToken(token);
        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        return new AuthToken(accessToken, refreshToken);
    }
}