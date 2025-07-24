using Framework.Common.Result;
using Framework.Domain;
using MongoDB.Driver;

namespace Framework.Database.MongoDB;

public class MongoDomainRepository<T>(IMongoDatabase mongoDatabase) : IDomainRepository<T> where T : class
{
    private readonly IMongoCollection<T> _collection = mongoDatabase.GetCollection<T>($"domain_{typeof(T).Name}");
    
    public async Task<Result> SaveAsync(T model)
    {
        try
        {
            var idFlter = Builders<T>.Filter.Eq("_id", (model as dynamic).Id);
            await _collection.ReplaceOneAsync(idFlter, model, new ReplaceOptions { IsUpsert = true });
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failed(ex);       
        }
    }

    public async Task<Result<T?>> GetByIdAsync<TEntityId>(TEntityId id) where TEntityId : IEntityId
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