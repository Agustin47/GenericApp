using Framework.Common;
using Framework.Database;

namespace Framework.Domain.Repository;

public abstract class DomainRepositoryEntity<TEntity>(Guid id, IRepositoryFactory repoFactory) : DomainEntity, IEntityState
    where TEntity : DomainEntity, IEntity
{
    public Guid Id { get; set; } = id;
    public int Version { get; set; }
    
    public override async Task SaveChanges()
    {
        var repoState = repoFactory.GetRepository<TEntity>();
        await repoState.UpdateAsync(this as TEntity);
    }
}