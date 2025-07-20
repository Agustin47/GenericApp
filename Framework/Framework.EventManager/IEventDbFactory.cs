namespace Framework.EventManager;

public interface IEventDbFactory
{
    IEventDb<TEvent> GetRepository<TEvent>() where TEvent : IEvent;
}