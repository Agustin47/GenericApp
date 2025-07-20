namespace Framework.EventManager;

public interface IEventDb<TEvent> where TEvent : IEvent
{
    Task SaveAsync(TEvent @event);
    Task<IEnumerable<TEvent>> GetFromIdAsync(string entityName, Guid id);
    Task<IEnumerable<TEvent>> GetFromIndexAsync(string entityName, Guid id, int startIndex);
}
