using Framework.CQRS.Queries;
using Framework.Database;
using Framework.Security;

namespace Framework.CQRS.Implementation;

public abstract class QueryBase<TResult> : IQuery<TResult>  where TResult : class
{
    public IFilter[]? Filters { get; set; }
    public ISorting? Sorting { get; set; }
    public IPaging? Paging { get; set; }
    public required UserContext UserContext { get; init; }
}