using Framework.Common;

namespace Framework.Database;

public interface IRepositoryFactory
{
    IRepository<T> GetRepository<T>(string? prefix = null) where T : IEntity;
}