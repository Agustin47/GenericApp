using Framework.Common.Result;

namespace Framework.CQRS.Queries;

public interface IQueryHandler<in TQuery, TResult>
    where TQuery : IQuery<TResult>
    where TResult : class
{
    Task<Result<TResult?>> Handle(TQuery command);
}