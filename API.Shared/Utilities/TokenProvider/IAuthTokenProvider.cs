using System.Security.Claims;
using Shared.Models;

namespace API.Shared.Utilities.TokenProvider;

public interface IAuthTokenProvider
{
    public AuthToken Create(List<Claim> claims, string audience);
}