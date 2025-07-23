using Framework.Common;
using Framework.Common.Result;

namespace Framework.Database;

public interface IDomainRepository<T> where T : class
{
    Task<Result> SaveAsync(T model);
    Task<Result<T?>> GetByIdAsync(Guid id);
}