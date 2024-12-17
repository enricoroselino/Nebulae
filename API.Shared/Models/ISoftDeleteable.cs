namespace API.Shared.Models;

public interface ISoftDeletable
{
    DateTime? DeletedOn { get; set; }
}