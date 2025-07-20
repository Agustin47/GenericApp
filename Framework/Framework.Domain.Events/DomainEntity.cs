using Framework.EventManager;

namespace Framework.Domain.Events;

public abstract class DomainEntity<TId> : DomainEntity where TId : IEntityId
{
    public TId Id { get;  private set; }

    public DomainEntity(TId id, IEventManager eventManager) : base(eventManager)
    {
        Id = id;
    }

    protected async Task PushEvent<TEvent>(TEvent @event) where TEvent : IEvent
    {
        @event.EntityId = Id.Value;
        @event.EntityName = GetType().Name;
        await _eventManager.PublishAsync(@event);
    }
}
public abstract class DomainEntity
{
    protected readonly IEventManager _eventManager; 

    public DomainEntity(IEventManager eventManager)
    {
        _eventManager = eventManager;
    }
    
    public abstract void Apply(IEvent @event);
}