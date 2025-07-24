using Framework.Common;
using Framework.Common.Result;
using Framework.Domain;

namespace Framework.Database;

public interface IDomainRepository<T> where T : class
{
    Task<Result> SaveAsync(T model);
    Task<Result<T?>> GetByIdAsync<TEntityId>(TEntityId id) where TEntityId : IEntityId;
}