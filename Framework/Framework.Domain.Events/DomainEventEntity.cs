using Framework.EventManager;

namespace Framework.Domain.Events;

public abstract class DomainEventEntity<TId>(TId Id, IEventManager eventManager) : DomainEventEntity where TId : IEntityId
{
    private List<IEvent> events = new();
    
    protected async Task PushEvent<TEvent>(TEvent @event) where TEvent : IEvent
    {
        @event.EntityId = Id.Value;
        @event.EntityName = GetType().Name;
        events.Add(@event);
    }

    public override async Task SaveChanges()
    {
        foreach (var @event in events)
            await eventManager.PublishAsync(@event);
    }
}
public abstract class DomainEventEntity : DomainEntity
{
    public abstract void Apply(IEvent @event);
}