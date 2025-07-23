using Framework.Database;

namespace Framework.Domain.Repository;

public class DomainRepositoryEntityFactory(IDomainRepositoryFactory repoDomainFactory, IRepositoryFactory repoFactory) : IDomainEntityFactory
{
    public TEntity Create<TEntity>(IEntityId id) where TEntity : DomainEntity
    {
        object[] constructorArgs = { id, repoDomainFactory, repoFactory };
        return (TEntity)Activator.CreateInstance(typeof(TEntity), constructorArgs);
    }

    public async Task<TEntity> GetByIdAsync<TEntity>(IEntityId id) where TEntity : DomainEntity
    {
        var repo = repoDomainFactory.GetRepository<TEntity>();
        var entity = await repo.GetByIdAsync(id.Value);
        return entity?.Value;
    }
}