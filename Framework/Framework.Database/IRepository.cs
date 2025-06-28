using System.Linq.Expressions;
using Framework.Common.Result;
using Framework.Specification;

namespace Framework.Database;

public interface IRepository<T>
{
    Task<Result> CreateAsync(T model);
    Task<Result> DeleteAsync(string id);
    
    Task<Result<List<T>>> Filter(QueryRepository<T> query);
    Task<Result<List<TR>>> Filter<TR>(QueryRepository<T> query, Func<T, TR> map);
    Task<Result<T>> FirstOrDefault(QueryRepository<T> query);
    Task<Result<TR>> FirstOrDefault<TR>(QueryRepository<T> query, Func<T, TR> map);
}