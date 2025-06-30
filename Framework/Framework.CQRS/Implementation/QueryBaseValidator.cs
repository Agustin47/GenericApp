using Framework.Common.Result;
using Framework.CQRS.Queries;

namespace Framework.CQRS.Implementation;

public class QueryBaseValidator<TQuery, TResult> : IQueryValidator<TQuery, TResult>
    where TQuery : IQuery<TResult>
    where TResult : class
{
    public Result ValidateQuery(TQuery command)
    {
        return Result.Success();
    }
}