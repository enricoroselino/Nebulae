namespace API.Shared.Models;

public interface ITimeAuditable
{
    public DateTime? CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}