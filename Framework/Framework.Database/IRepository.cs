using Framework.Common;
using Framework.Common.Result;

namespace Framework.Database;

public interface IRepository<T> where T : IEntity
{
    Task<Result> CreateAsync(T model);
    Task<Result> UpdateAsync(T model);
    Task<Result> DeleteAsync(Guid id);
    
    Task<Result<List<T>>> Filter(QueryRepository<T> query);
    Task<Result<List<TR>>> Filter<TR>(QueryRepository<T> query, Func<T, TR> map);
    Task<Result<T>> FirstOrDefault(QueryRepository<T> query);
    Task<Result<TR>> FirstOrDefault<TR>(QueryRepository<T> query, Func<T, TR> map);
}