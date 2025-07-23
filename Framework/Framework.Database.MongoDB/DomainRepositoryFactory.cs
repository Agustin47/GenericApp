using Framework.Common;
using MongoDB.Driver;

namespace Framework.Database.MongoDB;

public class DomainRepositoryFactory(IServiceProvider serviceProvider) : IDomainRepositoryFactory
{
    public IDomainRepository<T> GetRepository<T>() where T : class
    {
        IMongoDatabase database = (IMongoDatabase)serviceProvider.GetService(typeof(IMongoDatabase));
        return new MongoDomainRepository<T>(database);
    }
}