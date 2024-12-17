using BCrypt.Net;

namespace Shared.Utilities.Hasher;

/// <summary>
/// OWASP work factor minimum recommendation is 10, but 14 is recommended.<br/>
/// </summary>
public class BcryptHasher : IHasher
{
    private readonly int _workFactor;

    public BcryptHasher(int workFactor = 14)
    {
        _workFactor = workFactor;
    }

    public string Hash(string input)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(input, _workFactor, HashType.SHA512);
    }

    public bool VerifyHash(string input, string hashed)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(input, hashed, HashType.SHA512);
    }
}