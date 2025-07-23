using Framework.Common.Result;
using MongoDB.Driver;

namespace Framework.Database.MongoDB;

public class MongoDomainRepository<T>(IMongoDatabase mongoDatabase) : IDomainRepository<T> where T : class
{
    private readonly IMongoCollection<T> _collection = mongoDatabase.GetCollection<T>($"domain_{typeof(T).Name}");
    
    public async Task<Result> SaveAsync(T model)
    {
        try
        {
            await _collection.InsertOneAsync(model);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failed(ex);       
        }
    }

    public async Task<Result<T?>> GetByIdAsync(Guid id)
    {
        try
        {
            var idFlter = Builders<T>.Filter.Eq("_id", id);
            var value = (await _collection.Find(idFlter).ToListAsync())
                .FirstOrDefault();
            return Result.Success(value);
        }
        catch (Exception ex)
        {
            return Result.Failed(ex);
        }
    }
}