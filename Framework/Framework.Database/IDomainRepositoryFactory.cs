namespace Framework.Database;

public interface IDomainRepositoryFactory
{
    IDomainRepository<T> GetRepository<T>() where T : class;
}