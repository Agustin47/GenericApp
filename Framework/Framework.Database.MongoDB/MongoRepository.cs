using System.Linq.Expressions;
using Framework.Common.Result;
using MongoDB.Driver;

namespace Framework.Database.MongoDB;

public class MongoRepository<T> : IRepository<T>
{
    private readonly IMongoCollection<T> _collection;

    public MongoRepository(IMongoDatabase mongoDatabase)
    {
        _collection = mongoDatabase.GetCollection<T>(typeof(T).Name);
    }

    public async Task<Result> CreateAsync(T model)
    {
        await _collection.InsertOneAsync(model);
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(string id)
    {
        //await _collection.DeleteOneAsync(c => c == id);
        return Result.Success();
    }

    public async Task<Result<List<T>>> GetAsync(Expression<Func<T, bool>> filter)
    {
        var value = await _collection.Find(filter).ToListAsync();
        return Result.Success(value);
    }
}