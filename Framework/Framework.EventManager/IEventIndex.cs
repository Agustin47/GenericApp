namespace Framework.EventManager;

public class EventIndex
{
    public EventIndex()
    {
        Id = Guid.NewGuid();
        EntityNameId = string.Empty;
        Index = 0;
    }
    
    public EventIndex(string entityNameId)
    {
        Id = Guid.NewGuid();
        EntityNameId = entityNameId;
        Index = 0;
    }
    
    public Guid Id { get; set; }
    public string EntityNameId { get; set; }
    public int Index { get; set; }
}