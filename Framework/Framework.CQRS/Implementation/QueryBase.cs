using Framework.CQRS.Queries;
using Framework.Security;

namespace Framework.CQRS.Implementation;

public class QueryBase<TResult> : IQuery<TResult>  where TResult : class
{
    public UserContext UserContext { get; set; }
}