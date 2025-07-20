using MongoDB.Driver;

namespace Framework.EventManager.MongoDb;

public class EventIndexDbMongo(IMongoDatabase mongoDatabase) : IEventIndexDb
{
    private readonly IMongoCollection<EventIndex> _collection = mongoDatabase.GetCollection<EventIndex>($"event_{typeof(EventIndex).Name}");
    
    public async  Task<int> GetNextIndexAsync(string id, string entityName)
    {
        EventIndex index = await GetIndex(id, entityName);
        index.Index++;
        await _collection.ReplaceOneAsync(i => i.Id == index.Id, index);
        return index.Index;
    }

    public async Task<int> GetCurrentIndexAsync(string id, string entityName)
    {
        EventIndex eventIndex = await GetIndex(id, entityName);
        return eventIndex.Index;
    }

    public async Task SaveProjectionIndexAsync(string id, string entityName, int index)
    {
        EventIndex eventIndex = await GetIndex(id, entityName);
        eventIndex.Index = index;
        await _collection.ReplaceOneAsync(i => i.Id == eventIndex.Id, eventIndex);
    }

    private async Task<EventIndex> GetIndex(string id, string entityName)
    {
        string entityNameId = $"{entityName}:{id}";
        IEnumerable<EventIndex> collection = _collection.AsQueryable();
        var index = collection.FirstOrDefault(x => x.EntityNameId == entityNameId);
        if (index == null)
        {
            index = new(entityNameId);
            await _collection.InsertOneAsync(index);
        }
        return index;
    }
}