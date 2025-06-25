using MongoDB.Driver;

namespace Framework.Database.MongoDB;

public class RepositoryFactory : IRepositoryFactory
{
    private readonly IServiceProvider _serviceProvider;

    public RepositoryFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public IRepository<T> GetRepository<T>()
    {
        IMongoDatabase database = (IMongoDatabase)_serviceProvider.GetService(typeof(IMongoDatabase));
        return new MongoRepository<T>(database);
    }
}