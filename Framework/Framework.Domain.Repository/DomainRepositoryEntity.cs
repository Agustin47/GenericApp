using Framework.Common;
using Framework.Database;

namespace Framework.Domain.Repository;

public abstract class DomainRepositoryEntity<T, TEntity, TState>(T id, IDomainRepositoryFactory repoDomainFactory, IRepositoryFactory repoFactory) : DomainEntity 
    where T : IEntityId
    where TEntity : DomainEntity
    where TState : IEntityState
{
    public T Id => id;
    public override async Task SaveChanges()
    {
        var repo = repoDomainFactory.GetRepository<TEntity>();
        var repoState = repoFactory.GetRepository<TState>();
        await repo.SaveAsync(this as TEntity);
        await repoState.CreateAsync(ToState());
    }
    
    public abstract TState ToState();
}