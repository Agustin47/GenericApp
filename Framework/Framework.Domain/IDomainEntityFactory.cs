namespace Framework.Domain;

public interface IDomainEntityFactory
{
    TEntity Create<TEntity>(IEntityId id) where TEntity : DomainEntity;
    Task<TEntity> GetByIdAsync<TEntity>(IEntityId id) where TEntity : DomainEntity;
}