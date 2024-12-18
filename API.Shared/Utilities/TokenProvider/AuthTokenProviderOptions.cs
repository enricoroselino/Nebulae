using System.ComponentModel.DataAnnotations;

namespace API.Shared.Utilities.TokenProvider;

public class AuthTokenProviderOptions
{
    public const string Section = nameof(AuthTokenProviderOptions);
    private const int KeyLength = 64;

    [Required(ErrorMessage = "The Key is required.")]
    [StringLength(KeyLength, MinimumLength = KeyLength)]
    public string Key { get; init; } = null!;


    [Required(ErrorMessage = "The ValidIssuer is required.")]
    public string ValidIssuer { get; init; } = null!;

    [Required(ErrorMessage = "At least one ValidAudience is required.")]
    [MinLength(1, ErrorMessage = "At least one ValidAudience must be specified.")]
    public IEnumerable<string> ValidAudiences { get; init; } = [];

    [Required(ErrorMessage = "The ValidSpan is required.")]
    public TimeSpan ValidSpan { get; init; }
}