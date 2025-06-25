using Framework.Common.Result;

namespace Framework.CQRS.Queries;

public interface IQueryBus
{
    Task<Result<TResult?>> Handle<TResult>(dynamic query)
        where TResult : class;
}