namespace Framework.EventManager;

public interface IEvent<T> : IEvent
{
    T Body { get; set; }
}

public interface IEvent
{
    Guid Id { get; set; }
    Guid EntityId { get; set; }
    string EntityName { get; set; }
    string EventName { get; set; }
    string Username { get; set; }
    DateTimeOffset CreatedAt { get; set; }
    int Index { get; set; }
    string JsonBody { get; set; }
}

public abstract class Event : IEvent
{
    public Event()
    {
        Id = Guid.NewGuid();
    }
    public Guid Id { get; set; }
    public Guid EntityId { get; set; }
    public string EntityName { get; set; }
    public string EventName { get; set; }
    public string Username { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public int Index { get; set; }
    public string JsonBody { get; set; }
}

public abstract class Event<T> : Event, IEvent<T>
{
    public T Body { get; set; }
}