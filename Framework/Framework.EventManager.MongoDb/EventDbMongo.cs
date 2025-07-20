using MongoDB.Driver;

namespace Framework.EventManager.MongoDb;

public class EventDbMongo<TEvent>(IMongoDatabase mongoDatabase) : IEventDb<TEvent> where TEvent : IEvent
{
    private IMongoCollection<TEvent> GetCollection(string entityName, Guid entityId) => mongoDatabase.GetCollection<TEvent>($"event_{entityName}_{entityId}");
    
    public Task SaveAsync(TEvent @event)
    {
        var _collection = GetCollection(@event.EntityName, @event.EntityId);
        return _collection.InsertOneAsync(@event);
    }

    public async Task<IEnumerable<TEvent>> GetFromIdAsync(string entityName, Guid id)
    {
        var _collection = GetCollection(entityName, id);
        IEnumerable<TEvent> collection = _collection.AsQueryable();
        return collection.ToList();
    }

    public async Task<IEnumerable<TEvent>> GetFromIndexAsync(string entityName, Guid id,int startIndex)
    {
        var _collection = GetCollection(entityName, id);
        IEnumerable<TEvent> collection = _collection.AsQueryable();
        return collection.Where(x => x.Index > startIndex).ToList();
    }
}