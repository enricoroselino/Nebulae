namespace API.Modules.Identity.Models.Interfaces;

public interface IClaim
{
    public string ClaimType { get; }
    public string ClaimValue { get; }
}