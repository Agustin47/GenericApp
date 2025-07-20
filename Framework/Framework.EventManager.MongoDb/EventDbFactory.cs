using MongoDB.Driver;

namespace Framework.EventManager.MongoDb;

public class EventDbFactory(IServiceProvider provider) : IEventDbFactory
{
    public IEventDb<TEvent> GetRepository<TEvent>() where TEvent : IEvent
    {
        IMongoDatabase mongoDatabase = provider.GetService(typeof(IMongoDatabase)) as IMongoDatabase;
        return new EventDbMongo<TEvent>(mongoDatabase);
    }
}