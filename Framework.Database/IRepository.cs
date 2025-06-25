using System.Linq.Expressions;
using Framework.Common.Result;

namespace Framework.Database;

public interface IRepository<T>
{
    Task<Result> CreateAsync(T model);
    Task<Result> DeleteAsync(string id);
    
    Task<Result<List<T>>> GetAsync(Expression<Func<T, bool>> filter);
}