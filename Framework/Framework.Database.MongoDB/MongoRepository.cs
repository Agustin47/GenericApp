using System.Linq.Expressions;
using Framework.Common.Result;
using Framework.Specification;
using MongoDB.Driver;

namespace Framework.Database.MongoDB;

public class MongoRepository<T>(IMongoDatabase mongoDatabase) : IRepository<T>
{
    private readonly IMongoCollection<T> _collection = mongoDatabase.GetCollection<T>(typeof(T).Name);

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

    public Task<Result<List<T>>> Filter(QueryRepository<T> query) => Filter(query, x => x);
    public Task<Result<T>> FirstOrDefault(QueryRepository<T> query) => FirstOrDefault(query, x => x);
    
    public async Task<Result<TR>> FirstOrDefault<TR>(QueryRepository<T> query, Func<T, TR> map)
    {
        var element = ApplyQueryRepository(query, map).FirstOrDefault();
        return Result.Success(element);
    }

    public async Task<Result<List<TR>>> Filter<TR>(QueryRepository<T> query, Func<T, TR> map)
    {
        var elements = ApplyQueryRepository(query, map).ToList();
        return Result.Success(elements);
    }

    private IEnumerable<TR> ApplyQueryRepository<TR>(QueryRepository<T> query, Func<T, TR> map)
    {
        IEnumerable<T> collection = _collection.AsQueryable();

        if (query.Specifications != null && query.Specifications.Any())
        {
            var andCondition = Specification<T>.And(query.Specifications.ToArray());
            collection = collection.Where(andCondition.IsSatisfiedBy);
        }
        
        // if (query.Sorting != null)
        //     collection = query.Sorting.Ascending ? collection.OrderBy($"x => x.{query.Sorting.FieldName}");
        
        if(query.Paging != null)
            collection = collection
                .Skip(query.Paging.Index * query.Paging.Size)
                .Take(query.Paging.Size);

        return collection.Select(map);
    }
}