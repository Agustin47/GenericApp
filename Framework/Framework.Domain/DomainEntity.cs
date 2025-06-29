
namespace Framework.Domain;

public abstract class DomainEntity<TId> where TId : IEntityId
{
    public TId Id { get;  private set; }

    protected DomainEntity(TId id)
    {
        Id = id;
    }
}