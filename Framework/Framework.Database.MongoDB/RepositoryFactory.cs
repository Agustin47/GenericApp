using Framework.Common;
using MongoDB.Driver;

namespace Framework.Database.MongoDB;

public class RepositoryFactory(IServiceProvider serviceProvider) : IRepositoryFactory
{
    public IRepository<T> GetRepository<T>(string? prefix = null) where T : IEntity
    {
        IMongoDatabase database = (IMongoDatabase)serviceProvider.GetService(typeof(IMongoDatabase));
        return new MongoRepository<T>(database, prefix);
    }
}