using Framework.Specification;

namespace Framework.Database;

public interface IQueryRepositoryBuilder<T>
{
    IQueryRepositoryBuilder<T> AddFilters(params IFilter[]? filters);
    IQueryRepositoryBuilder<T> AddSpecs(params ISpecification<T>[]? filters);
    IQueryRepositoryBuilder<T> WithPaging(IPaging? paging);
    IQueryRepositoryBuilder<T> WithSorting(ISorting? paging);
    QueryRepository<T> Build();
}

public class QueryRepositoryBuilder<T>
    : IQueryRepositoryBuilder<T>
{
    private IPaging? _paging;
    private ISorting? _sorting;
    private readonly List<IFilter> _filters;
    private readonly List<ISpecification<T>> _specifications;

    private QueryRepositoryBuilder(List<IFilter> filters, List<ISpecification<T>> specifications, IPaging? paging,
        ISorting? sorting)
    {
        _filters = filters;
        _specifications = specifications;
        _paging = paging;
        _sorting = sorting;
    }

    
    public static QueryRepositoryBuilder<T> Create() => new(new(), new(), null, null);
    
    public IQueryRepositoryBuilder<T> AddFilters(params IFilter[]? filters1)
    {
        if (filters1 != null)
            _filters.AddRange(filters1);
        return this;
    }

    public IQueryRepositoryBuilder<T> AddSpecs(params ISpecification<T>[]? specifications1)
    {
        if(specifications1 != null)
            _specifications.AddRange(specifications1);
        return this;
    }

    public IQueryRepositoryBuilder<T> WithPaging(IPaging? paging)
    {
        _paging = paging;
        return this;
    }

    public IQueryRepositoryBuilder<T> WithSorting(ISorting? sorting)
    {
        _sorting = sorting;
        return this;
    }

    public QueryRepository<T> Build() => new(_filters, _specifications, _paging, _sorting);
}