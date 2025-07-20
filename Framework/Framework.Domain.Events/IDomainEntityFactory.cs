using Framework.EventManager;

namespace Framework.Domain.Events;

public interface IDomainEntityFactory
{
    TEntity Create<TEntity>(IEntityId id) where TEntity : DomainEntity;
    Task<TEntity> GetByIdAsync<TEntity>(IEntityId id) where TEntity : DomainEntity;
}

public class DomainEntityFactory(IEventManager eventManager) : IDomainEntityFactory
{
    public TEntity Create<TEntity>(IEntityId id) where TEntity : DomainEntity
    {
        object[] constructorArgs = { id, eventManager };
        return (TEntity)Activator.CreateInstance(typeof(TEntity), constructorArgs);
    }

    public async Task<TEntity> GetByIdAsync<TEntity>(IEntityId id) where TEntity : DomainEntity
    {
        var events = await eventManager.GetByIdAsync(typeof(TEntity).Name, id.Value);
        var entity = Create<TEntity>(id);
        foreach (var @event in events)
        {
            var type = Type.GetType(@event.EventName);
            var eventType = System.Text.Json.JsonSerializer.Deserialize(@event.JsonBody, type) as EventManager.Event;
            entity.Apply(eventType);
        }
        return entity;
    }
}