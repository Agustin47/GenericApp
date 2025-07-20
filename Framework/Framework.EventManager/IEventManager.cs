using Microsoft.Extensions.Logging;

namespace Framework.EventManager;

public interface IEventManager
{
    Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent;
    Task<IEnumerable<IEvent>> GetByIdAsync(string entityName, Guid id);
    Task Seed();
}


public class EventManager(IEventDbFactory dbFactory, IEventIndexDb eventIndexDb, IServiceProvider serviceProvider, ILogger<EventManager> logger) : IEventManager
{
    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent
    {
        int nextIndex = await eventIndexDb.GetNextIndexAsync(@event.EntityId.ToString(), @event.EntityName);
        @event.Index = nextIndex;
        @event.EventName = typeof(TEvent).AssemblyQualifiedName;
        @event.JsonBody = System.Text.Json.JsonSerializer.Serialize(@event);
        
        GenericEvent genericEvent = new GenericEvent(@event);
        var eventDb = dbFactory.GetRepository<GenericEvent>();
        await eventDb.SaveAsync(genericEvent);
        
        var projections = (serviceProvider.GetService(typeof(IEnumerable<IEventProjection<TEvent>>)) as
            IEnumerable<IEventProjection<TEvent>>)?.ToList();
        
        if(projections == null || !projections.Any())
            return;
        
        foreach(var projection in projections)
            await projection.HandleAsync(@event);
    }

    public async Task<IEnumerable<IEvent>> GetByIdAsync(string entityName, Guid id)
    {
        var eventDb = dbFactory.GetRepository<GenericEvent>();
        var events = await eventDb.GetFromIdAsync(entityName, id);
        return events.OrderBy(x => x.Index).ToList();
    }

    public async Task Seed()
    {
        var events  = serviceProvider.GetService(typeof(IEnumerable<IEvent>)) as object[];

        foreach (var @event in events)
        {
            // obtener los eventos
            // obtener los handlers
            // obtener el index
            // aplicar las projecciones desde el último indice.
            //@events.GetType().;
            var type = @event.GetType();
        }
    }

    private class GenericEvent : Event
    {
        public GenericEvent(IEvent @event) : base()
        {
            Id = @event.Id;
            EntityId = @event.EntityId;
            EntityName = @event.EntityName;
            EventName = @event.EventName;
            Username = @event.Username;
            CreatedAt = @event.CreatedAt;
            Index = @event.Index;
            JsonBody = @event.JsonBody;
        }
    }
}