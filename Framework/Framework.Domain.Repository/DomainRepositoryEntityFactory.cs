using Framework.Database;

namespace Framework.Domain.Repository;

public class DomainRepositoryEntityFactory(IDomainRepositoryFactory repoDomainFactory, IRepositoryFactory repoFactory) : IDomainEntityFactory
{
    public TEntity Create<TEntity>(IEntityId id) where TEntity : DomainEntity
    {
        object[] constructorArgs = { id, repoDomainFactory, repoFactory };
        return (TEntity)Activator.CreateInstance(typeof(TEntity), constructorArgs);
    }

    public async Task<TEntity> GetByIdAsync<TEntity, TEntityId>(TEntityId id)
        where TEntity : DomainEntity
        where TEntityId : IEntityId
    {
        var repo = repoDomainFactory.GetRepository<TEntity>();
        var entity = await repo.GetByIdAsync(id);
        if (entity?.Value == null) return null;
        
        var dest = Create<TEntity>(id);
        var type = typeof(TEntity);
        var properties = type.GetProperties().Where(x => x.CanWrite);
        foreach (var property in properties)
            property.SetValue(dest, property.GetValue(entity.Value, null));
        
        return dest;
    }

}