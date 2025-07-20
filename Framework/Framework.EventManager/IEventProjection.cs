namespace Framework.EventManager;

public interface IEventProjection<in TEvent> where TEvent : IEvent
{
    Task HandleAsync(TEvent @event);
}