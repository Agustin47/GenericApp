using Framework.Database;

namespace Framework.Domain.Repository;

public class DomainRepositoryEntityFactory(IRepositoryFactory repoFactory) : IDomainEntityFactory
{
    public TEntity Create<TEntity>(IEntityId id) where TEntity : DomainEntity
    {
        object[] constructorArgs = { id.Value, repoFactory };
        return (TEntity)Activator.CreateInstance(typeof(TEntity), constructorArgs);
    }

    public async Task<TEntity> GetByIdAsync<TEntity, TEntityId>(TEntityId id)
        where TEntity : DomainEntity
        where TEntityId : IEntityId
    {
        var factoryType = repoFactory.GetType();
        var getRepoMethod = factoryType.GetMethod("GetRepository");
        var genericRepoMethod = getRepoMethod.MakeGenericMethod(typeof(TEntity));
        var repo = genericRepoMethod.Invoke(repoFactory, [ null ]);
        
        var repoType = repo.GetType();
        var firstOrDefaultMethod = repoType.GetMethod("GetById");
        var genericGetByIdMethod = (TEntity)firstOrDefaultMethod.Invoke(repo, [ id.Value ]);
        if (genericGetByIdMethod == null)
            return null;
        
        var dest = Create<TEntity>(id);
        var type = typeof(TEntity);
        var properties = type.GetProperties().Where(x => x.CanWrite);
        foreach (var property in properties)
            property.SetValue(dest, property.GetValue(genericGetByIdMethod, null));
        
        return dest;
    }

}