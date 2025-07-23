namespace Framework.Domain;

public abstract class DomainEntity<TId> : DomainEntity where TId : IEntityId
{
    public TId Id { get;  protected set; }
}

public abstract class DomainEntity
{
    public abstract Task SaveChanges();
}

