namespace API.Shared.Models.DDD;

public interface IEntity : ITimeAuditable;

public interface IEntity<TKey> : IEntity
{
    public TKey Id { get; set; }
}

public abstract class Entity : IEntity
{
    public DateTime? CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}

public abstract class Entity<TId> : Entity, IEntity<TId>
{
    public TId Id { get; set; } = default!;
}