using Framework.Specification;

namespace Framework.Database;

public interface IQueryRepositoryBuilder<T>
{
    IQueryRepositoryBuilder<T> AddFilters(params IFilter[] filters);
    IQueryRepositoryBuilder<T> AddSpecs(params ISpecification<T>[]filters);
    IQueryRepositoryBuilder<T> WithPaging(IPaging paging);
    IQueryRepositoryBuilder<T> WithSorting(ISorting paging);
    QueryRepository<T> Build();
}

public class QueryRepositoryBuilder<T>(List<IFilter> filters, List<ISpecification<T>> specifications, IPaging? paging, ISorting? sorting)
    : IQueryRepositoryBuilder<T>
{

    public static QueryRepositoryBuilder<T> Create() => new(new(), new(), null, null);
    
    public IQueryRepositoryBuilder<T> AddFilters(params IFilter[] _filters)
    {
        filters.AddRange(_filters);
        return this;
    }

    public IQueryRepositoryBuilder<T> AddSpecs(params ISpecification<T>[] _specifications)
    {
        specifications.AddRange(_specifications);
        return this;
    }

    public IQueryRepositoryBuilder<T> WithPaging(IPaging _paging)
    {
        paging = _paging;
        return this;
    }

    public IQueryRepositoryBuilder<T> WithSorting(ISorting _sorting)
    {
        sorting = _sorting;
        return this;
    }

    public QueryRepository<T> Build() => new(filters, specifications, paging, sorting);
}